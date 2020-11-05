#!/bin/bash

GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m'

trap ctrl_c INT

function moveToProjectFolder() {
    cd ../../src
}

function runApis() {
    echo -e "$GREEN[!] Building Apis image...$NC"
    docker-compose build --no-cache
    echo -e "$GREEN[!] Running Apis...$NC"
    docker-compose up --force-recreate
}

function ctrl_c() {
    echo -e "$GREEN[!] Stopping Apis...$NC"
    docker-compose down
    exit 1
}

moveToProjectFolder
runApis
