# wowBattleSource
Игра с асинхронным пошаговым мультиплеером.

Требуется разработать игру с асинхронным пошаговым мультиплеером. Развернуть серверную часть игры на платформе облачных решений с возможностью масштабирования при
увеличении нагрузки.

## Архитектура решения

В качестве облачного сервиса, используется VK Cloud Solutions (Mail Cloud Solutions).

Для развертывания приложения, используються следующие сервисы:
  1) CDN для пользовательской части игры (Frontend).
  2) Кластеры Kubernetes для развертывания инстанций приложения игрового сервера.
  3) Балансировщик со стратегией LEAST CONNECTIONS для распределения нагрузки между развернутыми инстанциями игрового сервера.
  4) Кластер баз данных (MySQL) с синхронной репликацией для хранения информации о пользователях и проведенных боях.
  5) Hotbox бакет в объектном хранилище S3 для хранения медиа данных пользователей.
  6) Мониторинг состояния рабочих нод кластера Kubernetes и инстанций баз данных в кластере.
  
#Организация взаимодействия между сервисами на платформе облачных решений
![alt text](https://github.com/progML/wowBattleSource/blob/master/result/arch.jpg)

## Технологический стек
1) Серверное приложение представляет из себя контейнер с Python 3.9 и RESTFul API сервером, написанным с использованием фреймворка Flask и библиотек Flask-RestX, Flask-JWT-Extended. Работа с данными осуществляется при помощи ORM Peewee.
2) Фронтенд - приложение Unity WebGL.

## Демонстрация игры

Окно авторизации
![alt text](https://github.com/progML/wowBattleSource/blob/master/result/game_1.jpg)

Главное меню
![alt text](https://github.com/progML/wowBattleSource/blob/master/result/game_2.jpg)

Игровой процесс
![alt text](https://github.com/progML/wowBattleSource/blob/master/result/game_3.jpg)

Завершение битвы
![alt text](https://github.com/progML/wowBattleSource/blob/master/result/game_4.jpg)
