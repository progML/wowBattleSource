import typing as tp
import uuid
from dataclasses import dataclass

import werkzeug
import werkzeug.datastructures
from flask import Flask, Response, request, abort
from flask_cors import cross_origin
from flask_jwt_extended import JWTManager, jwt_required, get_jwt_identity, create_access_token, \
	exceptions as jwt_exceptions
from flask_restx import Resource, Api, fields, reqparse

# other modules
import config
import game
import model
import storage
# relative
from . import viewmodel

model.create_tables()
app = Flask(__name__)
storage = storage.get_file_storage() if config.STORAGE_TYPE == "file" else storage.get_s3_storage()

# header name: Authorization
# header type: Bearer
# format: Authorization: Bearer <token>
app.config["JWT_SECRET_KEY"] = "super-secret"  # Change this!
app.config["JWT_TOKEN_LOCATION"] = ['headers']
app.config["RESTX_MASK_SWAGGER"] = False
app.config["RESTX_JSON"] = {
	"cls": viewmodel.JSONEncoder
}
jwt = JWTManager(app)


# todo add all methods with get_user_by_login unauthorized with "user is None"
# Own restful-API with exception handlers

class ModifiedAPI(Api):
	custom_exception_handles: tp.Dict[tp.Type[Exception], tp.Callable[[Exception], Response]] = dict()

	def handle_error(self, error: Exception) -> Response:
		for handled_exception, handler in self.custom_exception_handles.items():
			if isinstance(error, handled_exception):
				return handler(error)
		else:
			return super().handle_error(error)

	def custom_error_handler(self, handled_exception: tp.Type[Exception]):
		def wrapper(fn):
			self.custom_exception_handles[handled_exception] = fn
			return fn

		return wrapper


authorizations = {
	'Bearer': {
		'type': 'apiKey',
		'name': 'Authorization',
		'in': 'header'
	}
}
api = ModifiedAPI(app, authorizations=authorizations, security='Bearer',
                  decorators=[cross_origin(send_wildcard=True)])


# ---
# -- Responses
# ---

@dataclass
class BasicResponse:
	success: bool
	message: str


basic_response_scheme = api.model('BasicResponse', {
	'success': fields.Boolean(True),
	'message': fields.String("OK"),
})


# --


@dataclass
class AuthData:
	token: str
	user_state: game.UserState


auth_data_scheme = api.model('AuthData', {
	"token": fields.String,
	"user_state": fields.String(enum=game.UserState._member_names_)
})


# --

@dataclass
class AuthDataResponse(BasicResponse):
	data: tp.Optional[AuthData]


auth_response_scheme = api.inherit('AuthResponse', basic_response_scheme, {
	'data': fields.Nested(auth_data_scheme, required=False)
})

# --

hit_data_scheme = api.model("HitData", {  # from game.HitData
	"hit_position": fields.Integer,
	"hit_result": fields.String(enum=game.HitState._member_names_)
})

# --

profile_data_scheme = api.model("ProfileData", {
	"login": fields.String,
	"user_state": fields.String,
})


@dataclass
class ProfileDataResponse(BasicResponse):
	data: tp.Optional[viewmodel.ProfileData]


profile_response_scheme = api.inherit("ProfileDataResponse", basic_response_scheme, {
	'data': fields.Nested(profile_data_scheme, required=False)
})

# --

base_data_scheme = api.model("BaseData", {
	"shields_bit_mask": fields.Integer,
})


@dataclass
class BaseDataResponse(BasicResponse):
	data: tp.Optional[viewmodel.BaseData]


base_response_scheme = api.inherit("BaseDataResponse", basic_response_scheme, {
	'data': fields.Nested(base_data_scheme, required=False)
})

# --

fight_data_scheme = api.model("FightData", {
	"user_id": fields.Integer,
	"fight_state": fields.String(enum=game.FightState._member_names_),
	"hit_data": fields.List(fields.Nested(hit_data_scheme)),
	"base_data": fields.Nested(base_data_scheme),
	"rest_hp": fields.Integer,
	"rest_hits": fields.Integer,
})


# --

@dataclass
class FightDataResponse(BasicResponse):
	data: tp.Optional[viewmodel.FightData]


fight_response_scheme = api.inherit('FightResponse', basic_response_scheme, {
	'data': fields.Nested(fight_data_scheme, required=False)
})

# --

gameconfig_data_scheme = api.model("GameConfigData", {
	"BASE_SLOTS": fields.Integer,
	"BASE_SHIELDS": fields.Integer,
	"FIGHT_DEFAULT_HP": fields.Integer,
	"FIGHT_MAX_HITS": fields.Integer
})


@dataclass
class GameConfigResponse(BasicResponse):
	data: game.GameConfigData


gameconfig_response_scheme = api.inherit('GameConfigResponse', basic_response_scheme, {
	'data': fields.Nested(gameconfig_data_scheme, required=False)
})


# --
# -- Exception handlers
# --

@api.custom_error_handler(jwt_exceptions.JWTExtendedException)
def handle_bad_request(error):
	return "Wrong house", 300


# --
# API Routes
# --

@app.before_request
def before_request():
	model.database.connect()


@app.after_request
def after_request(response):
	model.database.close()
	return response


@api.route("/game_config")
class GameConfig(Resource):
	@api.marshal_with(gameconfig_response_scheme)
	def get(self):
		return GameConfigResponse(True, "OK", game.game_config)


@api.route("/auth")
class Auth(Resource):
	@api.doc(security=None)
	@api.marshal_with(auth_response_scheme)
	@api.param("login", "User login", "query")
	@api.response(200, 'Success')
	@api.response(400, 'Params missing')
	# no password - don't need for testing
	def post(self):
		# todo: Проверить работу Auth:Post
		login_request = request.json.get('login', None)
		if login_request is None:
			return AuthDataResponse(False, "Missing `login` param", None), 400

		token = create_access_token(login_request)
		user = model.get_or_create_user(login_request)
		user_state: game.UserState = model.get_user_state(user)

		return AuthDataResponse(True, "OK", AuthData(token=token, user_state=user_state))


@api.route("/fight")
class Fight(Resource):
	method_decorators = [jwt_required()]

	@api.marshal_with(fight_response_scheme)
	@api.response(200, 'Success')
	@api.response(400, 'No available player to fight with')
	def get(self):
		login = get_jwt_identity()
		# todo: Проверить работу Fight:Get
		user = model.get_user_by_login(login)
		user_fight = model.get_or_create_user_fight(user)

		# Практически невозможно - только в ситуации, когда почему-то невовремя запросили (нет юзеров с базами)
		if user_fight is None:
			return FightDataResponse(False, "No available opponents, you've killed them all", None), 400
		return FightDataResponse(True, "OK", viewmodel.get_fight_data(user_fight))

	@api.marshal_with(fight_response_scheme)
	@api.param("position", "Hit position")
	@api.response(200, 'Success')
	@api.response(400, 'Params missing')
	@api.response(406, 'Invalid `position` param')
	def post(self):
		position = request.json.get('position', None)

		# None?
		if position is None:
			return FightDataResponse(False, "Missing `position` param", None), 400

		# Not int?
		try:
			position = int(position)
		except ValueError:
			return FightDataResponse(False, "Invalid `position` param", None), 406

		login = get_jwt_identity()
		# todo: Проверить работу Fight:Post
		user = model.get_user_by_login(login)
		user_fight = model.get_user_fight(user)
		if user_fight is None:
			return FightDataResponse(False, "No current fight", None), 400

		user_game_fight, _, base_data = viewmodel.get_fight(user_fight)  # get game fight object from DB object
		_ = user_game_fight.make_hit(position)  # make new hit, process fight
		viewmodel.update_fight_state(user_fight, user_game_fight)  # update DB
		new_list_of_hits = user_game_fight.hit_data  # get new hit data

		return FightDataResponse(
			True, "OK",
			viewmodel.get_fight_data(
				model_fight=user_fight,
				fight=user_game_fight,
				list_of_hits=new_list_of_hits,
				base_data=base_data
			)
		)


@api.route("/base/my")
class Base(Resource):
	method_decorators = [jwt_required()]

	@api.marshal_with(base_response_scheme)
	@api.response(200, 'Success')
	@api.response(400, 'No active base found')
	def get(self):
		login = get_jwt_identity()
		# todo: Проверить работу Base:Get
		user = model.get_user_by_login(login)
		base = model.get_user_base(user)
		if base is None:
			return BaseDataResponse(False, "No active base found", None), 400
		user_game_base, user_base_data = viewmodel.get_base_and_base_data(base)
		return BaseDataResponse(True, "OK", user_base_data)

	@api.marshal_with(base_response_scheme)
	@api.param("shields_bit_mask", "New bit mask for base (1 - shield up, 0 - shield down)")
	@api.response(200, 'Success')
	@api.response(400, 'Params missing')
	@api.response(406, 'Invalid `shields_bit_mask` param')
	def put(self):
		shields_bit_mask = int(request.json.get('shields_bit_mask', None))

		# None?
		if shields_bit_mask is None:
			return FightDataResponse(False, "Missing `shields_bit_mask` param", None), 400

		# Not int?
		try:
			shields_bit_mask = int(shields_bit_mask)
		except ValueError:
			return FightDataResponse(False, "Invalid `shields_bit_mask` param", None), 406

		login = get_jwt_identity()
		# todo: Проверить работу Base:Put
		# todo: Таймаут на создание базы
		user = model.get_user_by_login(login)
		print(shields_bit_mask)
		new_user_game_base = game.UserBase().from_int(shields_bit_mask)  # create game base from bit mask
		model.create_base(user, new_user_game_base.get_bytes())  # create model base from game base

		return BaseDataResponse(True, "OK", viewmodel.get_base_data(new_user_game_base))


@api.route("/profile/<int:user_id>")
@api.param("user_id", "User id")
class OtherProfile(Resource):
	method_decorators = [jwt_required()]

	@api.marshal_with(profile_response_scheme, description="Important: state is ALWAYS UNINITIALIZED")
	@api.response(200, 'Success')
	@api.response(400, 'User does not exist')
	def get(self, user_id: int):
		# login = get_jwt_identity()
		user = model.get_user_by_id(user_id)
		# todo: Проверить работу OtherProfile:Get
		if user is None:
			return ProfileDataResponse(False, "User does not exist", None)

		return ProfileDataResponse(True, "OK", viewmodel.get_profile_data(user))


@api.route("/profile/my")
class MyProfile(Resource):
	method_decorators = [jwt_required()]

	@api.marshal_with(profile_response_scheme)
	def get(self):
		login = get_jwt_identity()
		# todo: Проверить работу MyProfile:Get
		user = model.get_user_by_login(login)
		user_state: game.UserState = model.get_user_state(user)

		return ProfileDataResponse(True, "OK", viewmodel.get_profile_data(user, user_state))


@api.route("/photo/<int:user_id>")
class OtherProfilePic(Resource):
	method_decorators = [jwt_required()]

	@api.representation('image/jpeg')
	@api.response(200, 'Success')
	@api.response(400, 'User does not exist', basic_response_scheme)
	@api.response(404, 'Image not found', basic_response_scheme)
	def get(self, user_id: int):
		user = model.get_user_by_id(user_id)
		# todo: Проверить работу OtherProfile:Get
		if user is None:
			return BasicResponse(False, "User does not exist"), 400

		try:
			byte_array = storage.get(user.avatar_path)
			resp = app.make_response(byte_array)
			resp.headers['Content-Type'] = 'image/jpeg'
			return resp
		except:  # unable to get img
			return BasicResponse(False, "Image not found"), 404


file_upload = reqparse.RequestParser()
file_upload.add_argument('file',
                         type=werkzeug.datastructures.FileStorage,
                         location='files',
                         required=True,
                         help='image/jpeg file')


@api.route("/photo/my")
class MyProfilePic(Resource):
	method_decorators = [jwt_required()]

	@api.representation('image/jpeg')
	@api.response(200, 'Success')
	@api.response(404, 'Image not found', basic_response_scheme)
	def get(self):
		login = get_jwt_identity()
		user = model.get_user_by_login(login)
		# todo: протестировать MyProfilePic:Get
		try:
			byte_array = storage.get(user.avatar_path)
			resp = app.make_response(byte_array)
			resp.headers['Content-Type'] = 'image/jpeg'
			return resp
		except:  # unable to get img
			return abort(404)

	@api.expect(file_upload)
	@api.representation('image/jpeg')
	@api.response(200, 'Success')
	@api.response(400, 'Error with putting object', basic_response_scheme)
	@api.response(415, 'Wrong mime type', basic_response_scheme)
	def put(self):
		# basic_response_scheme
		login = get_jwt_identity()
		user = model.get_user_by_login(login)
		args: tp.Dict[str, werkzeug.datastructures.FileStorage] = file_upload.parse_args()
		if args['file'].mimetype == 'image/jpeg':
			try:
				img: werkzeug.datastructures.FileStorage = args["file"]
				img_bytes = img.read()
				img_name = str(uuid.uuid4())
				storage.put(img_name, img_bytes)

				user.avatar_path = img_name
				user.save()

				resp = app.make_response(img_bytes)
				resp.headers['Content-Type'] = 'image/jpeg'
				return resp
			except:  # Any exception from storage
				return BasicResponse(False, "Error with putting object"), 400
		else:
			return BasicResponse(False, "Wrong mime type"), 415
