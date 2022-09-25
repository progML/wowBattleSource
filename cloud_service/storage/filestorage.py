
import os.path
import typing as tp

# Abstract class
from .storage import Storage


class FileStorage(Storage):
	def __init__(self, root_path: str):
		self.root_path = root_path

	def put(self, key: str, data: bytes):
		file_path = os.path.join(self.root_path, key)
		print("FileStorage", file_path)
		with open(file_path, "wb") as f:
			f.write(data)

	def get(self, key: str) -> tp.Optional[bytes]:
		file_path = os.path.join(self.root_path, key)
		print("FileStorage", file_path)
		if os.path.isfile(file_path):
			with open(file_path, "rb") as f:
				return f.read()
		return None
