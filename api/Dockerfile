# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto e restaurar dependências
COPY ["SpassuDesafio.API/SpassuDesafio.API.csproj", "SpassuDesafio.API/"]
COPY ["SpassuDesafio.Application/SpassuDesafio.Application.csproj", "SpassuDesafio.Application/"]
COPY ["SpassuDesafio.Domain/SpassuDesafio.Domain.csproj", "SpassuDesafio.Domain/"]
COPY ["SpassuDesafio.Infrastructure/SpassuDesafio.Infrastructure.csproj", "SpassuDesafio.Infrastructure/"]

RUN dotnet restore "SpassuDesafio.API/SpassuDesafio.API.csproj"

# Copiar todo o código e compilar
COPY . .
WORKDIR "/src/SpassuDesafio.API"
RUN dotnet build "SpassuDesafio.API.csproj" -c Release -o /app/build

# Publicar a aplicação
FROM build AS publish
RUN dotnet publish "SpassuDesafio.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio Final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Criar diretório para o banco de dados SQLite e ajustar permissões
# No .NET 8, o usuário padrão é 'app'
USER root
RUN mkdir -p /app/data && chown -R app:app /app/data
USER app

COPY --from=publish /app/publish .

# Definir variáveis de ambiente padrão
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "SpassuDesafio.API.dll"]
