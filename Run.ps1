<#
.SYNOPSIS
Executa todos os targets de teste de cobertura e gera relatorio HTML consolidado.

.DESCRIPTION
Este script dispara os targets de teste definidos em cada projeto (sem repetir dotnet test)
e depois unifica os arquivos de cobertura em um relatorio unico, gerando a saida em /reports/report_TIMESTAMP/.

.PARAMETER RunMode
Test - executa cobertura individual por projeto e gera relatorio unico
App  - recompila a aplicacao via docker-compose com rebuild e sobe os containers

.EXAMPLE
.\run.ps1 -RunMode Test
Executa targets Test em todos os projetos de teste e gera o relatorio consolidado

.EXAMPLE
.\run.ps1 -RunMode App
Executa docker-compose down + build --no-cache + up

.NOTES
Autor: Weslley
Data: 2025-07-13
#>

param (
    [Parameter(Mandatory = $true)]
    [ValidateSet("App", "Test")]
    [string]$RunMode,
    [switch]$ViewReport
)

function Test-PathAvailable {
    param ([string]$cmd)
    $resolved = Get-Command $cmd -ErrorAction SilentlyContinue
    return $null -ne $resolved
}

function Require-Binary {
    param ([string]$name)
    if (-not (Test-PathAvailable $name)) {
        Write-Error "Dependencia '$name' nao encontrada. Verifique se esta instalada e acessivel no PATH."
        exit 1
    }
}

# ASCII banner
Write-Host ""
Write-Host "+-----------------------------------------+"
Write-Host "|          CDBCalculator Runner           |"
Write-Host "+-----------------------------------------+"
Write-Host ""

# Verifica dependencias basicas
foreach ($cmd in @("dotnet", "node", "npm", "npx", "reportgenerator")) {
    Require-Binary $cmd
}

# Valida versao do .NET
$dotnetVersion = dotnet --version
if ([version]$dotnetVersion -lt [version]"8.0.0") {
    Write-Error ".NET SDK 8.0 ou superior requerido. Versao atual: $dotnetVersion"
    exit 1
}

if ($RunMode -eq "App") {
    Write-Host "+-----------------------------------------+"
    Write-Host "|           App Docker Compose            |"
    Write-Host "+-----------------------------------------+"
    Write-Host "+-----------------------------------------+"
    Write-Host ""
    $composePath = Resolve-Path "docker-compose.yml" -ErrorAction SilentlyContinue
    if (-not $composePath) {
        Write-Error "Arquivo docker-compose.yml nao foi encontrado na raiz"
        exit 1
    }

    Write-Host "Encerrando containers anteriores..."
    docker compose down

    Write-Host "Executando build com --no-cache..."
    docker compose build --no-cache
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build falhou. Verifique os logs do Docker Compose."
        exit 1
    }

    Write-Host "Inicializando aplicacao..."
    docker compose up
    exit 0
}

if ($RunMode -eq "Test") {
    Write-Host "+-----------------------------------------+"
    Write-Host "|              Test Runner                |"
    Write-Host "+-----------------------------------------+"
    Write-Host "+-----------------------------------------+"
    Write-Host ""

    # Etapa 1: restaurar ferramentas declaradas no manifesto
    Write-Host "[1] Restaurando ferramentas..."
    dotnet tool restore

    # Etapa 2: executar targets de teste nos projetos
    Write-Host "[2] Executando testes nos projetos..."

    $testProjects = @(
        "tests/unit/Application/CDBCalculator.Application.UnitTests/CDBCalculator.Application.UnitTests.csproj",
        "tests/unit/Domain/CDBCalculator.Domain.UnitTests/CDBCalculator.Domain.UnitTests.csproj",
        "tests/unit/Infrastructure/CDBCalculator.Infrastructure.UnitTests/CDBCalculator.Infrastructure.UnitTests.csproj",
        "tests/unit/MicroServices/CDBCalculator.MicroService.UnitTests/CDBCalculator.MicroService.UnitTests.csproj",
        "tests/unit/ApiGateway/CDBCalculator.ApiGateway.UnitTests/CDBCalculator.ApiGateway.UnitTests.csproj"
    )
 
    foreach ($proj in $testProjects) {
        if (-not (Test-Path $proj)) {
            Write-Host " [X] Caminho nao encontrado: $proj"
            continue
        }

        Write-Host ""
        Write-Host "+-------------------------------+"
        Write-Host "| Projeto: $proj"
        Write-Host "+-------------------------------+"

        # Etapa 1: Clean
        Write-Host "[1] Limpando build antigo..."
        try {
            dotnet clean $proj
        } catch {
            Write-Host " [!] Falha ao limpar $proj. Pulando para o proximo."
            continue
        }

        # Etapa 2: Executar testes com cobertura
        Write-Host "[2] Executando testes com cobertura..."
        try {
            dotnet test $proj 
        } catch {
            Write-Host " [!] Falha ao executar testes em $proj. Pulando para o proximo."
            continue
        }

        Write-Host "[*] Testes concluídos para $proj"
    }

    # Etapa 3: gerar pasta de relatório consolidado
    Write-Host "[3] Gerando relatório unificado..."
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $reportDir = "reports\report_$timestamp"
    New-Item -ItemType Directory -Force -Path $reportDir | Out-Null

    # Etapa 4: consolidar os arquivos coverage.xml e gerar HTML
    Write-Host "[4] Executando reportgenerator..."
    $summaryPath = Join-Path $reportDir "summary.txt"

    reportgenerator `
      -reports:"tests\unit\**\**\TestResults\coverage.xml" `
      -targetdir:$reportDir `
      -reporttypes:Html `
      -title:"CDBCalculator - Cobertura Consolidada" `  
 
    Write-Host ""
    Write-Host "+-----------------------------------------+"
    Write-Host "| Relatório gerado: $reportDir"
    Write-Host "+-----------------------------------------+"
    Write-Host ""
    
    if ($ViewReport.IsPresent) {
        $htmlPath = Join-Path $reportDir "index.html"
        if (Test-Path $htmlPath) {
            Write-Host "[5] Abrindo relatório no navegador..."
            Start-Process $htmlPath
        } else {
            Write-Host "[!] Relatório HTML não encontrado em: $htmlPath"
        }
    }

    exit 0
}
