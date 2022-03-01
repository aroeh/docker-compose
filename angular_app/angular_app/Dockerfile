# Build context: /angular_app

# setting up the build image
FROM node:latest AS build
WORKDIR /source/app

# copy the angular project and src files to the build
# copy from repo source of angular_app to build destination of source/app/
# doing short hand syntax to say copy root source to destination root
COPY . .

# install app npm packages on the build image
RUN npm install

# Create a publish image using the build image as the base
FROM build AS publish

# build the app in the publish image using default environment configuration
RUN npm run build

# setting up the staging image for running the app and set the current working directory as app
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# copy the published files from the publish image directory
COPY --from=publish /dist .

# copy the nginx.conf from the source repository into the staging image
COPY nginx.conf /etc/nginx/nginx.conf