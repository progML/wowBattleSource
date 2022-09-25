import datetime as datetime
import typing as tp

from peewee import *

import config
import game

# database = SqliteDatabase('sqlite.db')
database = MySQLDatabase(config.DB_NAME, user=config.DB_USER, password=config.DB_PASSWORD,
                         host=config.DB_HOST)


class MyModel(Model):
	class Meta:
		database = database


class User(MyModel):
	login = CharField(unique=True)
	avatar_path = CharField(null=True)


class Base(MyModel):
	user = ForeignKeyField(User, backref="bases", index=True)
	data = BlobField()  # Any binary data we gonna store about our base (shields bit mask for now)


class Fight(MyModel):
	user = ForeignKeyField(User, backref="my_fights", index=True)
	opponent_user = ForeignKeyField(User, backref="fights_with_me", index=True)
	base = ForeignKeyField(Base, backref="fights_on_base")
	data = BlobField(default=b"[]")  # default empty array, [HitData()...] JSON String in bytes
	datetime = DateTimeField(default=datetime.datetime.now)
	state = CharField(default="NEW")  # fight state from fight_system.FightState


# -- Model

def create_tables():
	"""
	Создать таблицы в БД
	"""
	with database:
		database.create_tables([User, Fight, Base])


def get_user_by_id(user_id: int) -> tp.Optional[User]:
	"""
	Получить профиль по ID
	"""
	return User.get_or_none(id=user_id)


def get_user_by_login(login: str) -> tp.Optional[User]:
	"""
	Получить профиль по логину
	"""
	return User.get_or_none(login=login)


def get_or_create_user(login: str) -> User:
	"""
	Создает или получает из БД юзверя с данным логином
	"""
	try:
		with database.atomic():
			user = User.create(login=login)
	except IntegrityError:
		user = User.get(login=login)

	return user


def create_base(user: User, data: bytes) -> Base:
	"""
	Создать новую базу пользователя
	:param user: объект пользователя
	:param data: бинарные данные базы (1.0: байты по битовой маске базы)
	:return:
	"""
	base = Base.create(user=user, data=data)
	return base


def get_user_fight(user: User) -> tp.Optional[Fight]:
	"""
	Получить текущую (активную) битву пользователя
	"""
	user_fights = [fight for fight in user.my_fights.where(
		(Fight.state == game.FightState.IN_PROGRESS.value) |
		(Fight.state == game.FightState.NEW.value)
	)]
	if len(user_fights) > 0:
		current_fight = user_fights[0]  # Одновременно только один бой ОТ юзера может быть начат
		return current_fight
	else:
		return None


def get_user_base(user: User) -> tp.Optional[Base]:
	"""
	Получить активную (последнюю созданную) базу игрока
	"""
	user_bases_limit_1 = user.bases.order_by(Base.id.desc()).limit(1)
	if len(user_bases_limit_1) > 0:
		return user_bases_limit_1[0]
	else:
		return None


def get_or_create_user_fight(user: User) -> tp.Optional[Fight]:
	"""
	Получить текущий активный бой или создать новый с рандомным чепушем
	"""
	# Check if he has active fight
	current_user_fight = get_user_fight(user)
	if current_user_fight is not None:
		return current_user_fight

	# Get new opponent
	random_opponent_user = User.select().join(Base).order_by(fn.Rand()).limit(1)
	if len(random_opponent_user) == 0:
		return None

	# Create new fight
	random_opponent_user = random_opponent_user[0]
	base = get_user_base(random_opponent_user)
	new_fight = Fight.create(user=user, opponent_user=random_opponent_user, base=base)
	return new_fight


def get_user_state(user: User) -> game.UserState:
	"""
	Получить состояние пользователя по его данным и битвам
	"""
	if len([base for base in user.bases]) > 0 and user.avatar_path is not None:  # Есть база и есть аватар
		if get_user_fight(user):  # Есть начатая битва (не НОНЕ)
			return game.UserState.FIGHTING
		else:
			return game.UserState.FREE
	else:
		return game.UserState.UNINITIALIZED
