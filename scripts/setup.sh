#!/bin/bash
# Declaring constant values
readonly LOC="WestCentralUS"
readonly RESOURCE_GROUP="wpm"
readonly ACR_NAME="wpmacr693756"

echo "Setting up Azure resources in ${LOC}..."
readonly CREATE_GROUP="az group create --name ${RESOURCE_GROUP} --location ${LOC}"

echo "Creating Azure Container repo resource ${ACR_NAME}..."
readonly CREATE_ACR="az acr create --resource-group ${RESOURCE_GROUP} --name ${ACR_NAME} --sku Standard --location ${LOC} --admin-enabled true --role-assignment-mode rbac"

echo "Creating Azure Container Apps environment..."
readonly CREATE_CONTAINER_ENV="az containerapp env create --name wpmcontainerappenv --resource-group ${RESOURCE_GROUP} --location ${LOC} --logs-destination azure-monitor"

eval $CREATE_GROUP
eval $CREATE_ACR
eval $CREATE_CONTAINER_ENV

eval $CREATE_ACR # Force a change