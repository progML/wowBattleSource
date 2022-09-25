import dataclasses
import json
import typing as tp
from dataclasses import dataclass
from enum import Enum
from typing import Any, Iterator

import dacite
from dacite import from_dict

import game
import model
from game import HitState


def asdict_factory(data):
	def convert_value(obj):
		if isinstance(obj, Enum):
			return obj.value
		return obj

	return dict((k, convert_value(v)) for k, v in data)


class JSONEncoder(json.JSONEncoder):
	def default(self, o):
		if dataclasses.is_dataclass(o):
			print(o, dataclasses.asdict(o, dict_factory=asdict_factory))
			return dataclasses.asdict(o, dict_factory=asdict_factory)
		elif isinstance(o, Enum):
			return o.value
		return super().default(o)


@dataclass
class ProfileData:
	login: str
	user_state: game.UserState


@dataclass
class BaseData:
	shields_bit_mask: int  # bit-mask (0,0,0,1,1,1,0)


@dataclass
class FightData:
	user_id: int
	fight_state: game.FightState
	hit_data: tp.List[game.Fight.HitData]
	base_data: BaseData
	rest_hp: int
	rest_hits: int


def get_profile_data(model_user: model.User, game_user_state: tp.Optional[game.UserState] = None) -> ProfileData:
	profile_data = ProfileData(
		login=model_user.login,
		user_state=game_user_state if game_user_state is not None else game.UserState.UNINITIALIZED
	)
	return profile_data


def get_base_data(game_base: game.UserBase) -> BaseData:
	base_data = BaseData(shields_bit_mask=game_base.get_int())
	return base_data


def get_base_and_base_data(model_base: model.Base) -> tp.Tuple[game.UserBase, BaseData]:
	base = game.UserBase()
	base_data = BaseData(shields_bit_mask=base.from_bytes(model_base.data).get_int())
	return base, base_data


def get_fight(model_fight: model.Fight) -> tp.Tuple[game.Fight, tp.List[game.Fight.HitData], BaseData]:
	base, base_data = get_base_and_base_data(model_fight.base)
	loaded_hit_data = json.loads(model_fight.data.decode("utf-8"))

	list_of_hits = [
		from_dict(data_class=game.Fight.HitData, data=hit_dict,
		          config=dacite.Config(type_hooks={HitState: HitState}))
		for hit_dict in loaded_hit_data
	]

	fight = game.Fight(base, list_of_hits)
	return fight, list_of_hits, base_data


def update_fight_state(model_fight: model.Fight, game_fight: game.Fight):
	model_fight.state = game_fight.state.value  # current state of the fight
	model_fight.data = json.dumps(game_fight.hit_data, cls=JSONEncoder).encode("utf-8")  # bytes of JSON serialized hits
	model_fight.save()


def get_fight_data(model_fight: model.Fight = None, fight: game.Fight = None,
                   list_of_hits: tp.List[game.Fight.HitData] = None, base_data: BaseData = None) -> FightData:
	if fight == list_of_hits == base_data is None:
		fight, list_of_hits, base_data = get_fight(model_fight)

	fight_data = FightData(
		user_id=model_fight.opponent_user.id,
		fight_state=fight.state,
		hit_data=list_of_hits,
		base_data=base_data,
		rest_hp=fight.rest_hp,
		rest_hits=fight.rest_hits
	)
	return fight_data
