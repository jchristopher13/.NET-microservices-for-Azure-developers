#!/bin/bash
# Declaring constant values
readonly LOC="CentralUS"

# Ensure the LOCATION environment variable is set
if [[ -v LOCATION && -n "$LOCATION" ]]; then
    echo "Location is set to: $LOCATION"
else
    export LOCATION="CentralUS"
fi