# Envios Canarios

## Requirements
- Docker

## WebApp

### BackEnd

To run the WebAppApi for devel propouse you should go to `/src/WebApp` directory and run the following command:
`docker-compose up`

If the command finished successfuly status the Api is running in `http://127.0.0.1:5000`

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