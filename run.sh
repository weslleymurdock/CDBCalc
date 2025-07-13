#!/bin/bash

set -e

MODE=$1
VIEWREPORT=$2

# Checa argumento principal
if [[ "$MODE" != "Test" && "$MODE" != "App" ]]; then
  echo "Modo inválido. Use: ./run.sh [Test|App] [-view]"
  exit 1
fi

# Modo Test
if [[ "$MODE" == "Test" ]]; then
  echo "Executando testes com cobertura..."

  dotnet tool restore

  declare -a projects=(
    "tests/unit/Application/CDBCalculator.Application.UnitTests/CDBCalculator.Application.UnitTests.csproj"
    "tests/unit/Domain/CDBCalculator.Domain.UnitTests/CDBCalculator.Domain.UnitTests.csproj"
    "tests/unit/Infrastructure/CDBCalculator.Infrastructure.UnitTests/CDBCalculator.Infrastructure.UnitTests.csproj"
    "tests/unit/MicroServices/CDBCalculator.MicroService.UnitTests/CDBCalculator.MicroService.UnitTests.csproj"
    "tests/unit/ApiGateway/CDBCalculator.ApiGateway.UnitTests/CDBCalculator.ApiGateway.UnitTests.csproj"
  )

  for proj in "${projects[@]}"; do
    if [[ -f "$proj" ]]; then
      echo "Testando projeto: $proj"
      dotnet clean "$proj"
      dotnet msbuild "$proj" -target:Test
    else
      echo "Caminho não encontrado: $proj"
    fi
  done

  TIMESTAMP=$(date +"%Y%m%d_%H%M%S")
  REPORT_DIR="reports/report_$TIMESTAMP"
  mkdir -p "$REPORT_DIR"

  echo "Gerando relatório..."
  reportgenerator \
    -reports:"tests/unit/**/**/TestResults/coverage.xml" \
    -targetdir:"$REPORT_DIR" \
    -reporttypes:"Html;TextSummary;Badge" \
    -title:"CDBCalculator - Cobertura Consolidada"

  if [[ "$VIEWREPORT" == "-view" ]]; then
    INDEX="$REPORT_DIR/index.html"
    if [[ -f "$INDEX" ]]; then
      echo "Abrindo relatório..."
      xdg-open "$INDEX" || open "$INDEX"
    else
      echo "Relatório não encontrado."
    fi
  fi

  echo "Testes concluídos."
  exit 0
fi

# Modo App
if [[ "$MODE" == "App" ]]; then
  echo "Subindo aplicação via Docker Compose..."
  docker compose up --build
  exit 0
fi