# 🔹 Base de execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 🔧 Build e publicação da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["CDBCalc/src/MicroServices/CDBCalculator/CDBCalculator.csproj", "CDBCalc/src/MicroServices/CDBCalculator/"]
COPY ["CDBCalc/src/Application/CDBCalculator/Application.csproj", "CDBCalc/src/Application/CDBCalculator/"]
COPY ["CDBCalc/src/Domain/CDBCalculator/Domain.csproj", "CDBCalc/src/Domain/CDBCalculator/"]
COPY ["CDBCalc/src/Infrastructure/CDBCalculator/Infrastructure.csproj", "CDBCalc/src/Infrastructure/CDBCalculator/"]
RUN dotnet restore "./CDBCalc/src/MicroServices/CDBCalculator/CDBCalculator.csproj"
COPY . .

WORKDIR /src/CDBCalc/src/MicroServices/CDBCalculator
RUN dotnet publish "CDBCalculator.csproj" -c $BUILD_CONFIGURATION -o /app /p:UseAppHost=false

# 🔩 Etapa final: copia app  
FROM runtime AS final
WORKDIR /app 
COPY --from=builder /app . 
ENTRYPOINT ["dotnet", "CDBCalculator.dll"]