import boto3

s3 = boto3.resource(
	's3',
	endpoint_url="https://hb.bizmrg.com",
	aws_access_key_id="aPmX74hR6kmgLRFAjKJH5T",
	aws_secret_access_key="fqxuJGgYkSTK2me9Ccgcz3iXNMRUV1jnb5J31bjqihXx"
)

vip = s3.Bucket(name="gamecontent")
#
# with open(r"C:\users\admin\desktop\DIFICENTO.png", "rb") as f:
# 	vip.put_object(Key="dificento.png", Body=f.read())

dificento = vip.Object(key="5b06f0db-3b53-4e42-95dd-22e392892db7")
with open(r"C:\users\admin\desktop\DIFICENTO_back2.jpg", "wb") as f:
	f.write(dificento.get()["Body"].read())
