import os

# S3 Params
S3_ENDPOINT = os.environ.get("S3_ENDPOINT") or "http://192.168.0.109:9000"
S3_KEY_ID = os.environ.get("S3_KEY_ID") or "root"
S3_ACCESS_KEY = os.environ.get("S3_ACCESS_KEY") or "12345678"
S3_DEFAULT_BUCKET_NAME = os.environ.get("S3_DEFAULT_BUCKET_NAME") or "vip"

# File storage params
FILE_STORAGE_PATH = os.environ.get("FILE_STORAGE_PATH") or r"D:\Gamedev\GitRepos\cloud-2"

# Storage type
STORAGE_TYPE = os.environ.get("STORAGE_TYPE") or "file"  # or "s3"

# Database

DB_NAME = os.environ.get("DB_NAME") or "clouds"
DB_HOST = os.environ.get("DB_HOST") or "localhost"
DB_PORT = os.environ.get("DB_PORT") or "3306"
DB_USER = os.environ.get("DB_USER") or "root"
DB_PASSWORD = os.environ.get("DB_PASSWORD") or "Cloud1700Dollars"

