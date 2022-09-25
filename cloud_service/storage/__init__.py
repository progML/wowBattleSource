import config
from .storage import Storage


def get_s3_storage() -> Storage:
	from .s3storage import S3Storage
	return S3Storage(
		endpoint=config.S3_ENDPOINT,
		key_id=config.S3_KEY_ID,
		access_key=config.S3_ACCESS_KEY,
		default_bucket_name=config.S3_DEFAULT_BUCKET_NAME
	)


def get_file_storage() -> Storage:
	from .filestorage import FileStorage
	return FileStorage(root_path=config.FILE_STORAGE_PATH)
