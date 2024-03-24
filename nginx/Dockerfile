FROM ubuntu:latest as ssl

RUN apt-get update && apt-get install -y openssl
WORKDIR /ssl
RUN  openssl req -x509 -nodes -days 365 -subj  "/C=CA/ST=QC/O=Company Inc/CN=example.com" -newkey rsa:2048 -keyout nginx-selfsigned.key -out nginx-selfsigned.crt


FROM nginx:latest

COPY --from=ssl /ssl/nginx-selfsigned.crt /etc/nginx/ssl/nginx-selfsigned.crt
COPY --from=ssl /ssl/nginx-selfsigned.key /etc/nginx/ssl/nginx-selfsigned.key

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
EXPOSE 443
CMD ["nginx", "-g", "daemon off;"]