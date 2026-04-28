# TimeTracking System
Веб-приложение для автоматизации учета рабочего времени сотрудников. 
Проект демонстрирует создание полноценного Fullstack-решения с использованием REST API на стороне сервера и интерфейса на стороне клиента.

## 🛠 Технологический стек
* **Backend:** C# (.NET 8.0) / ASP.NET Core Web API
* **ORM:** Entity Framework Core (SQL Server Provider)
* **Frontend:** Vue.js 3 (Composition API), Axios, Bootstrap 5
* **API Documentation:** Swagger UI
* **СУБД:** Microsoft SQL Server

## 🚀 Запуск базы данных
Для работы приложения требуется SQL Server. Вы можете использовать локальный сервер или запустить его в Docker:
   ```powershell
      docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrongPassword123!" -p 1433:1433 --name timetracker-db -d [mcr.microsoft.com/mssql/server](https://mcr.microsoft.com/mssql/server)

## 🏗 Настройка и подготовка БД
Выполните команды в консоли диспетчера пакетов (Package Manager Console) в Visual Studio:
 ```powershell
    Update-Database

## 🏗 Архитектура проекта
Проект разделен на слои для обеспечения чистоты кода:

* **Models/** — Сущности базы данных (Project, WorkTask, TimeRecord).

* **Controllers/** — REST API эндпоинты. Реализована валидация данных (проверка лимита 24 часа в сутки).

* **Data/** — Контекст базы данных TimeTrackingContext и логика инициализации.

* **wwwroot/** — Клиентская часть приложения (SPA).
