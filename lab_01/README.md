# Лабораторная работа №1

## Подготовка

Описанные ниже инструкции приведены для `Fedora 43`.

[Туториал на PostgreSQL](https://www.postgresql.org/docs/current/tutorial.html)

### Установка PostgreSQL Fedora

- [Ссылка на официальную страницу скачивания](https://www.postgresql.org/download/)
- [Ссылка на гайд по установке для Red Hat Family](https://www.postgresql.org/download/linux/redhat/)

Добавляем официальный репозиторий PostgreSQL:

```bash
sudo dnf install -y https://download.postgresql.org/pub/repos/yum/reporpms/F-44-x86_64/pgdg-fedora-repo-latest.noarch.rpm
```

Устанавливаем сервер PostgreSQL

```bash
sudo dnf install -y postgresql18-server
```

Инициализируем кластер баз данных:

```bash
sudo /usr/pgsql-18/bin/postgresql-18-setup initdb
```

Проверяем, что все в порядке:

```cpp
arist0crab@fedora:~$ sudo systemctl status postgresql-18
[sudo] password for arist0crab: 
○ postgresql-18.service - PostgreSQL 18 database server
     Loaded: loaded (/usr/lib/systemd/system/postgresql-18.service; disabled; preset: disabled)
    Drop-In: /usr/lib/systemd/system/service.d
             └─10-timeout-abort.conf
     Active: inactive (dead)
       Docs: https://www.postgresql.org/docs/18/static/
```

### Установка DBeaver Fedora

Сразу скажу, что если скачивать с официального сайта `rpm` пакет и устанавливать его вкучную, то скорее всего вы столкнетесь с тем, что ваша `java` версия не нравится бобру, или он считает, что у вас нет самой `java`. Чтобы не забираться в тонкости подкачки 17-й версии `java`, т.к. судя по всему грызуну нужна именно она, можно скачивать с `flatpak`, тогда системная `java` вообще не нужна будет.

Устанавливаем `Flatpak`:

```bash
sudo dnf install flatpak
```

Добавляем репозиторий `flathub`:

```sh
flatpak remote-add --if-not-exists flathub https://flathub.org/repo/flathub.flatpakrepo
```

Устанавливаем `DBeaver`:

```bash
flatpak install flathub io.dbeaver.DBeaverCommunity
```

После этого `DBeaver` должен появиться в меню приложений.