import typing as tp
from abc import ABC, abstractmethod


class Storage(ABC):
	@abstractmethod
	def put(self, key: str, data: bytes):
		pass

	@abstractmethod
	def get(self, key: str) -> tp.Optional[bytes]:
		pass

