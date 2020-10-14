#!/bin/bash

GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m'

trap ctrl_c INT

function moveToWebAppApiFolder() {
    cd ../../src/WebApp/backend
}

function runWebAppApi() {
    echo -e "$GREEN[!] Building WebAppApi image...$NC"
    docker-compose build --no-cache
    echo -e "$GREEN[!] Running WebAppApi...$NC"
    docker-compose up --force-recreate
}

function ctrl_c() {
    echo -e "$GREEN[!] Stopping WebAppApi...$NC"
    docker-compose down
    exit 1
}

moveToWebAppApiFolder
runWebAppApi