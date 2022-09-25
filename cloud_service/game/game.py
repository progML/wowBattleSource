from __future__ import annotations

import typing as tp
from dataclasses import dataclass
from enum import Enum


# config

@dataclass
class GameConfigData:
	BASE_SLOTS: int
	BASE_SHIELDS: int
	FIGHT_DEFAULT_HP: int
	FIGHT_MAX_HITS: int


game_config = GameConfigData(
	BASE_SLOTS=10,
	BASE_SHIELDS=5,
	FIGHT_DEFAULT_HP=3,
	FIGHT_MAX_HITS=5,
)


# Common

def int_to_bytes(x: int) -> bytes:
	return x.to_bytes((x.bit_length() + 7) // 8, 'big')


def int_from_bytes(xbytes: bytes) -> int:
	return int.from_bytes(xbytes, 'big')


# --

class UserState(str, Enum):
	UNINITIALIZED = "UNINITIALIZED"
	FREE = "FREE"
	FIGHTING = "FIGHTING"


# No user class - don't need it

# --

class UserBase:
	default_base: tp.List[bool] = [True for _ in range(game_config.BASE_SHIELDS)] + \
	                              [False for _ in range(game_config.BASE_SLOTS - game_config.BASE_SHIELDS)]

	def __init__(self, shield_data: tp.List[bool] = None):
		if shield_data is None:
			shield_data = self.default_base
		self.shield_data = shield_data

	@property
	def shield_data(self):
		return self._shield_data

	@shield_data.setter
	def shield_data(self, value: tp.List[bool]):
		if not UserBase.is_valid(value):
			value = self.default_base

		self._shield_data = value
		self.shield_int_data = self.get_int()
		self.shield_bytes_data = self.get_bytes()

	# Serialize

	def get_int(self) -> int:
		int_data = sum([2 ** i if self.shield_data[i] else 0 for i in range(game_config.BASE_SLOTS)])
		return int_data

	def get_bytes(self) -> bytes:
		int_data = self.get_int()
		return int_to_bytes(int_data)

	# Deserialize

	def from_int(self, int_data: int) -> UserBase:
		self.shield_data = [(int_data >> i) % 2 == 1 for i in range(game_config.BASE_SLOTS)]
		return self

	def from_bytes(self, bytes_data: bytes) -> UserBase:
		int_data = int_from_bytes(bytes_data)
		return self.from_int(int_data)

	# Valid check

	@staticmethod
	def is_valid(value: tp.List[bool]) -> bool:
		slots = len(value)
		shields = len([s for s in value if s])
		return slots == game_config.BASE_SLOTS and shields == game_config.BASE_SHIELDS


class HitState(str, Enum):
	MISS = "MISS"
	BREAK = "BREAK"
	SAME = "SAME"


class FightState(str, Enum):
	NEW = "NEW"
	IN_PROGRESS = "IN_PROGRESS"
	WIN = "WIN"
	LOSE = "LOSE"


class Fight:
	@dataclass
	class HitData:
		hit_position: int
		hit_result: HitState

	def __init__(self, base_data: UserBase, hit_data: tp.List[HitData] = None):
		if hit_data is None:
			hit_data = list()

		# init base, hit_data and default hp
		self.base_data = base_data
		self.hit_data = hit_data

		# if there are hits, process
		if len(hit_data) > 0:
			self.init_hits()
		else:
			self.state: FightState = FightState.NEW
			self.success_hits = 0
			self.rest_hits = game_config.FIGHT_MAX_HITS

	def init_hits(self):
		# process all hits
		self.rest_hits = game_config.FIGHT_MAX_HITS - len(self.hit_data)
		self.success_hits = len([hit for hit in self.hit_data if hit.hit_result == HitState.BREAK])

		# process game
		self.process_fight()

	def make_hit(self, position: int) -> tp.Optional[HitData]:
		"""
		Ударить по полю и получить объект удара
		:param position: позиция, куда бьем
		:return: объект удара (позиция: инт, стейт)
		"""

		# Позиция вне поля
		if not (0 <= position < game_config.BASE_SLOTS):
			return None

		# Расчет """удачности""" удара
		if position in [hit.hit_position for hit in self.hit_data]:
			hit_state = HitState.SAME
		elif self.base_data.shield_data[position]:
			hit_state = HitState.BREAK
		else:
			hit_state = HitState.MISS

		# create hit
		hit = Fight.HitData(position, hit_state)
		self.hit_data.append(hit)

		# process hit
		self.rest_hits -= 1
		if hit == HitState.BREAK:
			self.success_hits += 1

		# process game
		self.process_fight()
		return hit

	@property
	def rest_hp(self):
		return game_config.FIGHT_DEFAULT_HP - self.success_hits

	def process_fight(self):
		if self.success_hits >= game_config.FIGHT_DEFAULT_HP:  # Попал раз больше или равно, чем ХП
			self.state = FightState.WIN
		elif self.rest_hits > 0:  # Если как бы хп еще есть и удары есть
			self.state = FightState.IN_PROGRESS
		else:  # ХП есть, ходов нет
			self.state = FightState.LOSE
