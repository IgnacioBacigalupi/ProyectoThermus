FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

FROM nginx:alpine

COPY --from=build /app/dist /tmp/dist

RUN set -eux; \
    rm -rf /usr/share/nginx/html/*; \
    BROWSER_DIR="$(find /tmp/dist -maxdepth 3 -type d -name browser | head -n 1)"; \
    if [ -n "$BROWSER_DIR" ]; then \
      cp -r "$BROWSER_DIR"/* /usr/share/nginx/html/; \
    else \
      cp -r /tmp/dist/* /usr/share/nginx/html/; \
    fi; \
    if [ -f /usr/share/nginx/html/index.csr.html ]; then \
      mv /usr/share/nginx/html/index.csr.html /usr/share/nginx/html/index.html; \
    fi

COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80