# Use a imagem base do SDK do .NET 8 para construir a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos de projeto e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante dos arquivos da aplicação
COPY . .
WORKDIR /app/src/PebaFinance.Api

# Publica a aplicação para produção
RUN dotnet publish -c Release -o out

# Use a imagem de runtime do .NET 8 para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/src/PebaFinance.Api/out .

# Expõe a porta que a aplicação irá escutar (o Railway injeta a variável PORT)
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

# Define o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "PebaFinance.Api.dll"] 
