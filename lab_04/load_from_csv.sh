#!/bin/bash

PASSWORD="$1"

sudo docker exec -i mssql-server /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "$PASSWORD" -C -d lwdb -i /tmp/load_from_csv_mssql.sql