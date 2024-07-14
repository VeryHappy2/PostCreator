# FROM node:latest AS builder
# WORKDIR /app
# COPY Web/post-web/package.json Web/post-web/package-lock.json ./

# RUN npm install

# COPY Web/post-web .

# RUN npm run build

# FROM nginx:alpine
# COPY --from=builder /app/dist/frontend /usr/share/nginx/html
FROM node:latest AS builder
WORKDIR /dist/src/app
# Copy files to virtual directory
# COPY package.json package-lock.json ./
# Run command in Virtual directory
RUN npm cache clean --force
# Copy files from local machine to virtual directory in docker image
COPY . .
RUN npm install
RUN npm run build --prod


### STAGE 2:RUN ###
# Defining nginx image to be used
FROM nginx:latest AS ngi
# Copying compiled code and nginx config to different folder
# NOTE: This path may change according to your project's output folder 
COPY --from=build /dist/src/app/dist/post-web /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]