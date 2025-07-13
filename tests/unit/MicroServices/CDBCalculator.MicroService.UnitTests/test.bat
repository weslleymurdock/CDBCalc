@echo off
echo Executando testes funcionais do Microservice...

REM Apagar resultados anteriores
rmdir /S /Q TestResults 2>nul
rmdir /S /Q CoverageReport 2>nul

REM Executar testes com cobertura e sem shadow copy (runsettings desativando AppDomain)
dotnet test --settings MicroserviceTests.runsettings --collect:"XPlat Code Coverage"

REM Gerar relatório de cobertura HTML
reportgenerator ^
  -reports:"TestResults/**/coverage.cobertura.xml" ^
  -targetdir:"CoverageReport" ^
  -reporttypes:Html ^
  -title:"CDBCalculator – Functional Coverage" ^
  -assemblyfilters:+CDBCalculator

REM Abrir relatório no navegador padrão
start CoverageReport\index.html
echo Relatório de cobertura gerado com sucesso em CoverageReport\index.html