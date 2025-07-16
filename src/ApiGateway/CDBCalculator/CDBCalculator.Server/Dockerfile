# 🔹 Etapa base para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

 
# 🔹 Etapa para construir SPA Angular
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS spa-builder
WORKDIR /spa
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get update && apt-get install -y nodejs

COPY src/ApiGateway/CDBCalculator/cdbcalculator.client/cdbcalculator.client.esproj cdbcalculator.client/
COPY src/ApiGateway/CDBCalculator/cdbcalculator.client/ cdbcalculator.client/
WORKDIR /spa/cdbcalculator.client
RUN npm install && npm run build --prod

# 🔹 Etapa para compilar e publicar o projeto ASP.NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS gateway-builder
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY src/ApiGateway/CDBCalculator/CDBCalculator.Server/CDBCalculator.Server.csproj CDBCalculator.Server/
COPY src/ApiGateway/CDBCalculator/CDBCalculator.Server/ CDBCalculator.Server/
RUN dotnet restore CDBCalculator.Server/CDBCalculator.Server.csproj
WORKDIR /src/CDBCalculator.Server
RUN dotnet publish CDBCalculator.Server.csproj -c $BUILD_CONFIGURATION -o /publish /p:UseAppHost=false /p:BuildingInsideDocker=true
 
FROM runtime AS final
WORKDIR /app
RUN mkdir -p ./wwwroot
COPY --from=gateway-builder /publish .
COPY --from=spa-builder /spa/cdbcalculator.client/dist/cdbcalculator.client/browser/* ./wwwroot/ 
 
ENTRYPOINT ["dotnet", "CDBCalculator.Server.dll"]