import config

config.DB_HOST = "37.139.40.122"
config.DB_USER = "dificento"
config.DB_PASSWORD = "mudila_gorokhoviy_1488"
config.DB_NAME = "VIP"

import model

model.create_tables()
model.User.create(login="Player")
print(model.User.get_or_none(login="Player"))
