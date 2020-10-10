# Envios Canarios

## Requirements
- Docker

## WebApp

### BackEnd

To run the WebAppApi for devel propouse you should go to `/src/WebApp` directory and run the following command: `docker-compose up --force-recreate`

If the command finished successfuly the Api is running in `http://127.0.0.1:5000`

#### Database

To avoid losing the local database data the database container creates a directory that he use as a volume. This directory is located in `~/envios-canarios/db-data`.
If you remove this directory you will lose all your local database data.