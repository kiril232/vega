# build
FROM node:20-alpine AS build

WORKDIR app/

COPY package*.json .
RUN npm ci

COPY . .

ARG VITE_USER_API=/users-api
ARG VITE_PRODUCT_API=/products-api
ARG VITE_CART_API=/cart-api
ARG VITE_ORDER_API=/orders-api

ENV VITE_USER_API=$VITE_USER_API
ENV VITE_PRODUCT_API=$VITE_PRODUCT_API
ENV VITE_CART_API=$VITE_CART_API
ENV VITE_ORDER_API=$VITE_ORDER_API

RUN npm run build

# serve
FROM nginx:1.27-alpine

COPY --from=build /app/dist /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

