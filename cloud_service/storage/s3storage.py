import typing as tp

# Amazon lib for bucket tricks
import boto3

# Abstract class
from .storage import Storage


class S3Storage(Storage):
	def __init__(self, endpoint: str, key_id: str, access_key: str, default_bucket_name: str = None):
		self.s3 = boto3.resource(
			's3',
			endpoint_url=endpoint,
			aws_access_key_id=key_id,
			aws_secret_access_key=access_key
		)

		# Get default bucket
		if default_bucket_name is None:
			self.default_bucket = None
		else:
			self.default_bucket = self.get_bucket(default_bucket_name)

	def get_bucket(self, bucket_name: str):
		return self.s3.Bucket(name=bucket_name)

	def put(self, key: str, data: bytes, bucket_name: str = None):
		"""
		Положить объект в бакет по ключу
		Нет экспешпшенов - все прошло успешно
		"""
		if bucket_name is not None:
			bucket = self.get_bucket(bucket_name)
		else:
			bucket = self.default_bucket

		bucket.put_object(Key=key, Body=data)

	def get(self, key: str, bucket_name: str = None) -> tp.Optional[bytes]:
		"""
		Получить объект из бакета по ключу
		"""
		if bucket_name is not None:
			bucket = self.get_bucket(bucket_name)
		else:
			bucket = self.default_bucket

		obj = bucket.Object(key=key)
		return obj.get()["Body"].read()
