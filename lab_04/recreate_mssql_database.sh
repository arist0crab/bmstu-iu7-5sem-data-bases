#!/bin/bash

PASSWORD="$1"

sudo docker exec -i mssql_server /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "$PASSWORD" -C \
    -Q "ALTER DATABASE lwdb SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE lwdb;"

sudo docker exec -i mssql_server /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "$PASSWORD" -C \
    -Q "CREATE DATABASE lwdb;"

sudo docker exec -i mssql_server /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "$PASSWORD" -C -d lwdb -i /tmp/creating.sql

sudo docker exec -i mssql_server /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "$PASSWORD" -C -d lwdb -i /tmp/restrictions.sql