#!/bin/bash
# Declaring constant values
readonly LOC="CentralUS"
readonly RESOURCE_GROUP="wpm"
readonly ACR_NAME="wpmacr693756"

readonly CREATE_GROUP="az group create --name ${RESOURCE_GROUP} --location ${LOC}"

readonly CREATE_ACR="az acr create --resource-group ${RESOURCE_GROUP} --name ${ACR_NAME} --sku Standard --location ${LOC} --admin-enabled true --role-assignment-mode rbac"

eval $CREATE_GROUP
eval $CREATE_ACR # Force a change