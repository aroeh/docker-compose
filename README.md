# Introduction

This repository is an experiment to see if we can simulate a local development setup to run multiple different apps with docker compose.  For example in an enterprise setting, there are typically more than repository and application that make up a system.  A developer will have each of those repos cloned on their local machine and might need many/all of them running.  So I wanted to try to recreate the same here.

## Goals

1. Explore more in depth local setups with docker compose
2. Create a setup to simulate different apps from different repositories and run them all with docker compose

## References

- [Docker Getting Started](https://docs.docker.com/get-started/)
- [Docker Compose Context](https://docs.docker.com/compose/compose-file/compose-file-v3/#context)
- [aroeh Docker Quickstart Repo](https://github.com/aroeh/docker-quickstart)

# Tools

- Windows 10 Pro **Pro or higher is needed since that enables the hyper-v virtualization on windows hosts
- Visual Studio 2022 v17.0.5
- Visual Studio Code v1.63.2
- Docker
    - Desktop v4.4.4
    - Engine v20.10.12
    - Compose v1.29.2
- .Net6
- Node.js v17.6.0
- npm v8.5.2
- AngularCLI v13.2.4

# Getting Started

Make sure to understand docker build context before running the code or attempting to edit the docker-compose.yml or the app Dockerfile.  Context is the key for how the dockerfiles and compose are setup and work.  The aroeh docker quickstart has some info on that, but also be sure to reference the Docker documentation for more details.

# Solution Overview

There are 3 different solutions in the repository to account for different apps and technologies.  Each one has it's own Dockerfile for containerization.  Currently there are 2 .net apps and 1 angular app

Folder structure is as follows
```
├── .gitignore
├── docker-compose.yml
├── angular_app
│   ├── angular_app
│   │   ├── node_modules
│   │   ├── src
│   │   │   ├── App Code
│   │   ├── angular.json
│   │   ├── package.json
├── blazor_app
│   ├── blazor_app.sln
│   ├── blazor_app
│   │   ├── blazor_app.csproj
│   │   ├── Dockerfile
│   │   ├── .dockerignore
│   │   ├── nginx.conf
│   │   ├── Other app files
├── weather_api
│   ├── weather_api.sln
│   ├── weather_api
│   │   ├── weather_api.csproj
│   │   ├── Dockerfile
│   │   ├── .dockerignore
│   │   ├── nginx.conf
│   │   ├── Other app files
```

Each solution can be run independently via an IDE or images/containers for debugging purposes.  They can be run together at the same time via docker-compose.

# Build and Run

Each project can be built independently but docker and docker compose are the intended ways build all of the projects.  Use a Command line tool that can run docker commands

1. Make sure Docker is running
2. Change to the solution directory containing the .sln file
```
cd <PATH>\docker-multi-repo-sim
```
3. Run docker compose up
```
docker-compose up
```

> NOTE: To run in detached mode use the following command
```
docker-compose up -d
```

4. View the apps running in Docker

> NOTE: The redirect for the blazor app here isn't perfect and doesn't work yet on http://localhost:4002
> The redirect does work for http://localhost:4001

If making any changes to a project, the container and image will need to rebuilt.  I found it easiest to remove all images using docker compose down
```
docker-compose down --rmi 'all'
```

# Docker Commands

Review the docker command line reference at [Docker Docs](https://docs.docker.com/reference/)

These are the most common commands I found myself using in this project
| Command | Example | Reference Documentation | Notes |
|---------|---------|-------------------------|-------|
| docker-compose up [options] | docker-compose up -d | [Docker Compose Up](https://docs.docker.com/compose/reference/up/) | adding the -d will run in detached mode and is great for faster startup.  This is also preferred if not needing to debug |
| docker-compose down [options] | docker-compose down --rmi 'all'| [Docker Compose Down](https://docs.docker.com/compose/reference/down/) | When needing to make changes to a project you will have to rebuild the image and container.  This command made it easy to tear everything down very quickly to a clean state.  There might be a better way to do this though, but this was easy to use |
| docker logs -f <container-name> | docker logs -f docker_quickstart_sln-cache-1 | [Docker Logs](https://docs.docker.com/engine/reference/commandline/logs/) | If the container is nested under a solution, then use the nester container name and not the name of the image as set in the compose file |
| docker run [options] <image-name> |docker run redis| [Docker Logs](https://docs.docker.com/engine/reference/commandline/logs/) | This was handy for pulling down a single image to do some basic setup and testing before incorporating into the project and compose |
| docker exec [options] [container] [command] | docker-compose -i docker-multi-repo-sim-cache-1 | [Docker exec](https://docs.docker.com/engine/reference/commandline/exec/) | Container and commands will depened on what you want or need to do.  For example you can redis cli commands in a docker redis container |

## Docker Redis Commands
| Command | Example |
|---------|---------|
| docker exec -i <container-name> redis-cli FLUSHALL | docker exec -i docker-multi-repo-sim-cache-1 redis-cli FLUSHALL |