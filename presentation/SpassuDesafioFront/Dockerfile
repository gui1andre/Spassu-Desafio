# Estágio de Build
FROM node:20-alpine AS build
WORKDIR /app

# Argumento para a URL da API (pode ser passado no build)
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL

# Instalar dependências
COPY package*.json ./
RUN npm install

# Copiar código e gerar build
COPY . .
RUN npm run build

# Estágio de Produção (NGINX)
FROM nginx:stable-alpine
COPY --from=build /app/dist /usr/share/nginx/html

# Configuração customizada do Nginx para suportar roteamento SPA (React Router)
RUN printf "server { \n\
    listen 80; \n\
    location / { \n\
        root /usr/share/nginx/html; \n\
        index index.html index.htm; \n\
        try_files \$uri \$uri/ /index.html; \n\
    } \n\
}" > /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
