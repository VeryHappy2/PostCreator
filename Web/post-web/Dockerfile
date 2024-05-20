FROM node:latest AS builder
WORKDIR /app
COPY Web/post-web/package.json Web/post-web/package-lock.json ./

RUN npm install -g @angular/cli
RUN npm install

COPY Web/post-web .

RUN ng build --output-path=dist --configuration=production

FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]