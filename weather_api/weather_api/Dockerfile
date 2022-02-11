# The context refers to the source directory for the build
# This file is setup to run with the project file as the context
# docker build -t weatherimage .
# COPY sources use either the file name or . since the context is set as the directory containing the .csproj file

# Build context: weather_api/weather_api

# setting up the build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy the .csproj from the context directory to the image directory
# copy weather_api/weather_api/weather_api.csproj to the build image directory /source/output
COPY weather_api.csproj output/

# restore packages on the api project from the image directory
RUN dotnet restore output/weather_api.csproj

# copy the remaining api and lib project files
# copy everything in weather_api/weather_api to the build image directory /source/output
COPY . output/

# set the working directory to build image directory /source/project
WORKDIR /source/output

# build the api and lib projects for release configuration
RUN dotnet build weather_api.csproj -c release

# Get the files from the build image and publish them for release
# output the published files into the /app directory
FROM build AS publish
RUN dotnet publish -c release --no-build -o /app

# setting up the staging image for running the app and set the current working directory as app
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# copy the files from the published image app directory to the stage image app directory
COPY --from=publish /app .

# set the entry point command and dll for the app in the image
ENTRYPOINT ["dotnet", "weather_api.dll"]