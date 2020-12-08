# Canary Deliveries

## Requirements

- Docker >=19.03
- **CANARY_DELIVERIES_HOME** environment var decalared with the project root path

## Table of content

1. [Web App](#web-app)
   1. [BackEnd](#web-app-backend)
      * [Api Documentation](#web-app-backend-api-doc)
      * [Health Checks](#web-bapp-backend-health)
   2. [FrontEnd](#web-bapp-frontend)   
2. [Backoffice](#backoffice)
   1. [BackEnd](#backoffice-backend)
       * [Api Documentation](#backoffice-backend-api-doc)
3. [Database](#database)
4. [Technology](#technology)

## Web App

<a name="web-app-backend"></a>
### BackEnd

To run the WebAppApi for devel propouse you should go to `/scripts/devel` directory and execute the `run-apis.sh` script. **CANARY_DELIVERIES_HOME** enviroment var is needed to execute the script, see the Requirements sections for more info.

`run-apis.sh` applies the required database migrations automatically.

If the script execution finished successfuly, the Api is running in `http://192.168.2.2`

**To stop the Api press CTRL+C**

<a name="web-app-backend-api-doc"></a>
#### Api Documentation

The Api documentation is created following the OpenApi specification (OAS3), you can view it using the following route: `http://192.168.2.2/_doc`.

**The Api documentations is only available in a development enviroment.**

<a name="web-bapp-backend-health"></a>
#### Health Checks (`/health`)

Health checks runs the following infraestructure connections:
* Postgres

<a name="web-bapp-frontend"></a>
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

<a name="backoffice"></a>
## Backoffice

<a name="backoffice-backend"></a>
### BackEnd

To run the BackofficeApi for devel propouse you should go to `/scripts/devel` directory and execute the `run-apis.sh` script. **CANARY_DELIVERIES_HOME** enviroment var is needed to execute the script, see the Requirements sections for more info.

`run-apis.sh` applies the required database migrations automatically.

If the script execution finished successfuly, the Api is running in `http://192.168.2.5`

**To stop the Api press CTRL+C**

<a name="backoffice-backend-api-doc"></a>
#### Api Documentation

The Api documentation is created following the OpenApi specification (OAS3), you can view it using the following route: `http://192.168.2.5/_doc`.

**The Api documentations is only available in a development enviroment.**

<a name="database"></a>
## Database

The database container creates a directory that it uses as a volume to avoid losing the local data. This directory is located in `~/envios-canarios/db-data`.
If you remove this directory you will lose all your local database data.

<a name="technology"></a>
## Technology

* .NET Core 3.0
* Postgres 13.1
* Nuxt.js
* TypeScript
