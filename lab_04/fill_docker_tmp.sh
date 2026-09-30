#!/bin/bash

sudo docker cp creating_mssql.sql mssql_server:/tmp/creating.sql
sudo docker cp restrictions_mssql.sql mssql_server:/tmp/restrictions.sql
sudo docker cp load_from_csv_mssql.sql mssql_server:/tmp/load_from_csv.sql

sudo docker cp output_csv/. mssql_server:/tmp/

sudo docker cp release/. mssql_server:/tmp/