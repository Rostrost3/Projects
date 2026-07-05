# HabitTracker

HabitTracker — это приложение для отслеживания привычек

## Сделано

* Регистрация и аутентификация пользователей с использованием JWT
* Создание, редактирование и удаление привычек
* Безопасное хеширование паролей
* База данных SQL Server с миграциями Entity Framework Core
* Поддержка Docker и Docker Compose
* Сохранение ключей защиты данных с использованием томов Docker

## Технологии

* ASP.NET Core 10
* Blazor Interactive Server
* Entity Framework Core
* SQL Server
* Docker & Docker Compose
* JWT Authentication

## Запуск приложения

### Предварительные требования

* Docker Desktop

### Конфигурация

Создайте файл `.env` в корневой директории проекта, используя `.env.example`

### В папке с решением выполните команду

```bash
docker compose up --build
```

Приложение будет доступно по адресу:

```
http://localhost:5001
```

SQL Server будет доступен по адресу:

```
localhost:5003
```

## Примечания

* Миграции базы данных применяются автоматически при запуске приложения.
* Данные SQL Server хранятся в томе Docker.
* Ключи защиты данных ASP.NET Core сохраняются в томе Docker.
