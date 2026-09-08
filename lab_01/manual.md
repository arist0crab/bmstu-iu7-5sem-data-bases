# Небольшой мануал для меня и для вас

Команды приведены в соответствии с моей версией операционной системы и версией самого PostgreSQL.

## Работа с локальным сервером

### Старт сервера

```bash
sudo systemctl start postgresql-18
```

### Остановка сервера

```bash
sudo systemctl stop postgresql-18
```

### Проверка статуса сервера

```bash
sudo systemctl status postgresql-18
```

## Создание нового пользователя

Создать нового пользователя для базы данных можно как sql командой, так и с помощью командной строки postresql. Различия не столь значительны, в основном они в том, что с помощью sql команды нельзя создать суперпользователя. [Страничка по теме со всеми возомножностями](https://www.postgresql.org/docs/current/app-createuser.html)

Однако создавать нового пользователя в правами администратора не обязтельно, потому что можно подключиться от имени `postgres` - суперпользователя PostgreSQL. К этому пользователю необходимо будет задать пароль, если вы этого не делали, можно исполнить это вручную.

Используя оболочку `psql`, позволяющую выполнять `sql`-запросы фактически из командной строки, залетаем внутрь с помощью:

```bash
sudo -u postgres psql
```

Теперь с помощью самого `sql` запроса можно будет поменять пароль пользователю:

```sql
ALTER USER postgres WITH PASSWORD 'your_new_password';
```

## Создание новой базы данных

От лица того же самого суперпользователя создаем БД:

```bash
sudo -u postgres createdb newdb
```

Или через ту же оболочку `psql`:

```bash
arist0crab@fedora:~$ sudo -u postgres psql
psql (18.6)
Type "help" for help.

postgres=# CREATE DATABASE some_db;
CREATE DATABASE
```

Теперь, когда база данных создана, к ней можно подключиться.

## Основные команды для работы с базой данных

### Создание новой таблицы

- [английская версия](https://www.postgresql.org/docs/current/tutorial-table.html)
- [русская версия](https://postgrespro.ru/docs/postgresql/18/tutorial-table)

### Редактирование созданной таблицы 

- [английская версия](https://www.postgresql.org/docs/current/sql-altertable.html)
- [русская версия](https://postgrespro.ru/docs/postgresql/current/sql-altertable)