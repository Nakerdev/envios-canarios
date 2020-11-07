#!/bin/bash

GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m'

trap ctrl_c INT

function moveToProjectFolder() {
    if [ -z "${CANARY_DELIVERIES_HOME}" ]; then
        echo -e "$RED[ERR] CANARY_DELIVERIES_HOME env var does not exist$NC"
        exit 1
    fi
    cd $CANARY_DELIVERIES_HOME/src
}

function runApis() {
    echo -e "$GREEN[INFO] Building Apis image...$NC"
    docker-compose build --no-cache
    echo -e "$GREEN[INFO] Running Apis...$NC"
    docker-compose up --force-recreate
}

function ctrl_c() {
    echo -e "$GREEN[INFO] Stopping Apis...$NC"
    docker-compose down
    exit 1
}

moveToProjectFolder
runApis
