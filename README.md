# Envios Canarios

## Requirements
- Docker
- **CANARY_DELIVERIES_HOME** envirement var decalared with the project root path

## WebApp

### BackEnd

To run the WebAppApi for devel propouse you should go to `/scripts/devel` directory and execute the `run-web-app-api.sh` script. To execute the script need the **CANARY_DELIVERIES_HOME** enviroment var declared, see the Requirements sections for more info.

If the script execution finished successfuly, the Api is running in `http://127.0.0.1:5000`

**To stop the Api press CTRL+C**

### Documentation

The Api documentation is created following the OpenApi specification (OAS3), you can view it using the following route: `http://127.0.0.1:5000/_doc`.

**The Api documentations is only available in a development enviroment.**

#### Database

To avoid losing the local database data the database container creates a directory that he use as a volume. This directory is located in `~/envios-canarios/db-data`.
If you remove this directory you will lose all your local database data.

### FrontEnd

Go to `/src/WebApp/frontend` directory

- Launch a development server on localhost:3000 with hot-reloading:
`npm run dev`

If the command finished successfuly the WebApp is running in `http://127.0.0.1:3000`

- Serve your production application from dist/ directory:
    1. `npm run build`
    2. `npm run start`

If the command finished successfuly the WebApp is running in `http://127.0.0.1:3000`

- To generate the static website you can run the following command:
`npm run generate`
