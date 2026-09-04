FROM node:22-alpine AS build
WORKDIR /app
COPY src/task-manager-web/package*.json ./
RUN npm ci
COPY src/task-manager-web/ .
RUN npm run build

FROM nginx:1.28-alpine
COPY src/task-manager-web/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/task-manager-web/browser /usr/share/nginx/html
EXPOSE 80
