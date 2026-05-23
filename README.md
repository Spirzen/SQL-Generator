# SQL Generator

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/platform-Windows-0078D4?logo=windows&logoColor=white)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE.txt)

**SQL Generator** — настольное WPF-приложение для визуальной сборки SQL-запросов. Вы задаёте таблицу, столбцы и условия в форме — приложение формирует готовый SQL, который можно скопировать в SSMS, DBeaver, pgAdmin или любой другой клиент.

Подходит для обучения SQL, быстрых прототипов и проверки синтаксиса без ручного набора каждого ключевого слова.

---

## Содержание

- [Возможности](#возможности)
- [Требования](#требования)
- [Быстрый старт](#быстрый-старт)
- [Как пользоваться](#как-пользоваться)
- [Примеры запросов](#примеры-запросов)
- [Архитектура](#архитектура)
- [Структура проекта](#структура-проекта)
- [Лицензия](#лицензия)

---

## Возможности

| Категория | Поддержка |
|-----------|-----------|
| Типы запросов | `SELECT`, `INSERT`, `UPDATE`, `DELETE` |
| Соединения | `INNER`, `LEFT`, `RIGHT`, `FULL` JOIN |
| Фильтрация | `=`, `<>`, `>`, `<`, `>=`, `<=`, `LIKE`, `IS NULL`, `IS NOT NULL` |
| SELECT | `DISTINCT`, `ORDER BY` (ASC / DESC), `LIMIT` |
| Значения | Числа без кавычек, строки в `'...'`, экранирование `'`, `NULL`, `TRUE` / `FALSE` |
| Интерфейс | Двухколоночный layout, тёмная панель SQL, подсказки в полях, статусная строка |
| Удобство | Копирование в буфер, очистка формы, скрытие JOIN для не-SELECT |

---

## Требования

- **ОС:** Windows 10 / 11
- **SDK:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) с поддержкой WPF (`net8.0-windows`)

---

## Быстрый старт

Клонируйте репозиторий и выполните в каталоге с `.csproj`:

```bash
dotnet restore
dotnet run --project "SQL Generator.csproj"
```

Сборка Release:

```bash
dotnet build -c Release
```

Готовое приложение: `bin/Release/net8.0-windows/SQL Generator.exe`

---

## Как пользоваться

1. Выберите **тип запроса** (по умолчанию `SELECT`).
2. Укажите **имя таблицы** (например, `users`).
3. Введите **столбцы** через запятую: `id, name, email`.
4. Для `INSERT` и `UPDATE` заполните **значения** в том же порядке, что и столбцы.
5. При необходимости настройте **WHERE** (столбец, оператор, значение).
6. Для `SELECT` дополнительно доступны:
   - **JOIN** — тип, таблица, столбец связи;
   - **ORDER BY** — столбец и направление (ASC / DESC);
   - **LIMIT** — максимальное число строк;
   - флаг **SELECT DISTINCT**.
7. Нажмите **Сгенерировать**, затем **Скопировать в буфер** при необходимости.

> Для `IS NULL` и `IS NOT NULL` поле значения отключается автоматически.

Кнопка **Очистить** сбрасывает все поля формы.

---

## Примеры запросов

### SELECT

| Поле | Значение |
|------|----------|
| Таблица | `users` |
| Столбцы | `id, name, email` |
| WHERE | `status` `=` `active` |
| ORDER BY | `name` ASC |
| LIMIT | `10` |

```sql
SELECT id, name, email
FROM users
WHERE status = 'active'
ORDER BY name ASC
LIMIT 10;
```

### INSERT

| Столбцы | `name, email` |
| Значения | `John, john@example.com` |

```sql
INSERT INTO users (name, email)
VALUES ('John', 'john@example.com');
```

### UPDATE

| Столбцы | `name, status` |
| Значения | `Jane, active` |
| WHERE | `id` `=` `5` |

```sql
UPDATE users
SET name = 'Jane', status = 'active'
WHERE id = 5;
```

### JOIN (SELECT)

| JOIN | `INNER` |
| Таблица | `orders` |
| Столбец | `user_id` |
| Связь с | `id` (в `users`) |

```sql
SELECT id, name, email
FROM users
INNER JOIN orders ON orders.user_id = users.id;
```

---

## Архитектура

Паттерн **MVVM**:

```text
View (MainWindow.xaml)
        |  data binding
ViewModel (MainViewModel)
        |  commands, validation
Helpers (QueryFactory, SqlValueFormatter)
        |
Models (SelectQueryBuilder, InsertQueryBuilder, ...)
```

| Слой | Назначение |
|------|------------|
| **View** | XAML, стили, привязки — без бизнес-логики |
| **ViewModel** | Состояние формы, генерация WHERE, команды |
| **Models** | Fluent Builder для каждого типа SQL |
| **Helpers** | Фабрика билдеров, форматирование литералов, `ICommand` |

`SqlValueFormatter` отвечает за корректные литералы: числа, строки с экранированием, `NULL`, булевы значения.

---

## Структура проекта

```text
SQL Generator/
??? Models/
?   ??? QueryBuilder.cs
?   ??? SelectQueryBuilder.cs
?   ??? InsertQueryBuilder.cs
?   ??? UpdateQueryBuilder.cs
?   ??? DeleteQueryBuilder.cs
??? ViewModels/
?   ??? MainViewModel.cs
??? Views/
?   ??? MainWindow.xaml
?   ??? MainWindow.xaml.cs
??? Helpers/
?   ??? Command.cs
?   ??? QueryFactory.cs
?   ??? SqlValueFormatter.cs
??? Converters/
?   ??? Converters.cs
?   ??? VisibilityConverters.cs
??? App.xaml
??? SQL Generator.csproj
```

---

## Лицензия

[MIT](LICENSE.txt) — Copyright (c) 2025 Timur Tagirov
