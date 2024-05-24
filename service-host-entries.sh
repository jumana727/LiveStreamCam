#!/bin/bash

DOCKER_COMPOSE_FILE="docker-compose.yml"
CONTAINER_IP="127.0.0.1"
HOSTS_FILE="/etc/hosts"

# Parse command-line options
if [ $# -eq 0 ]; then
    echo "Usage: $0 <add|remove> [-f DOCKER_COMPOSE_FILE] [-i CONTAINER_IP] [-o HOSTS_FILE]" >&2
    exit 1
fi

ACTION=$1
shift

# Parse command-line options
while getopts ":f:i:o:" opt; do
  case $opt in
    f) DOCKER_COMPOSE_FILE="$OPTARG"
    ;;
    i) CONTAINER_IP="$OPTARG"
    ;;
    o) HOSTS_FILE="$OPTARG"
    ;;
    \?) echo "Invalid option -$OPTARG" >&2
    ;;
  esac
done

# Extract service names from Docker Compose file
SERVICE_NAMES=$(docker compose -f $DOCKER_COMPOSE_FILE config --services)

if [ "$ACTION" == "add" ]; then
    for service in $SERVICE_NAMES
    do
        if ! grep -q -E "\b$service\b" $HOSTS_FILE; then
            echo "$CONTAINER_IP $service" | sudo tee -a $HOSTS_FILE >/dev/null
        fi
    done
elif [ "$ACTION" == "remove" ]; then
    for service in $SERVICE_NAMES
    do
        sudo cp $HOSTS_FILE $HOSTS_FILE.tmp
        sudo grep -v -E "\b$service\b" $HOSTS_FILE.tmp | sudo tee $HOSTS_FILE >/dev/null
        rm $HOSTS_FILE.tmp
    done
else
    echo "Invalid action. Specify 'add' or 'remove' as the first argument." >&2
    echo "Usage: $0 <add|remove> [-f DOCKER_COMPOSE_FILE] [-i CONTAINER_IP] [-o HOSTS_FILE]" >&2
    exit 1
fi
