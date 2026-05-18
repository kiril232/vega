#!/bin/bash
set -e

# create one db per microservice; the .net services run migrations on boot
for db in vega_users vega_products vega_carts vega_orders; do
  psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
    CREATE DATABASE $db;
EOSQL
done
