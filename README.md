# 💸 CDBCalc

Calculadora de CDB com arquitetura modular, testes unitários, CI/CD e cobertura consolidada de código.  
Este projeto visa demonstrar boas práticas em APIs financeiras com .NET e Angular, com foco em confiabilidade, mensuração de qualidade e automação.

---

## 📦 Tecnologias utilizadas

- [.NET SDK 8.0.403](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20.x](https://nodejs.org)
- [Angular CLI](https://angular.io/cli) (via `npx`)
- [Coverlet (msbuild)](https://github.com/coverlet-coverage/coverlet)
- [ReportGenerator](https://danielpalme.github.io/ReportGenerator/)
- PowerShell 7+ (para automação via `Run.ps1`)
- GitHub Actions (CI)
- GitHub Pages (CD para o relatório)

---

## 🧰 Requisitos para execução local

| Ferramenta     | Versão mínima  | Verificação                       |
|----------------|----------------|-----------------------------------|
| .NET SDK       | `8.0.403`      | `dotnet --version`                |
| Node.js        | `20.x`         | `node --version`                  |
| Angular CLI    | via `npx`      | `npx ng version`                  |
| PowerShell     | `7.0+`         | `$PSVersionTable.PSVersion`       |

---


## 🚀 Como usar o projeto

### 1. 📥 Clonar o repositório

#### 🖥️ Windows (PowerShell):


Clone o repositório em C:\repo\ ou ~/repo em Linux ou OSX:

```powershell
git clone https://github.com/weslleymurdock/CDBCalc.git
cd CDBCalc
```


#### 🐧 Linux / 🍎 macOS (Bash/Zsh):

```bash
mkdir -p ~/repo
cd ~/repo
git clone https://github.com/weslleymurdock/CDBCalc.git
cd CDBCalc
```


### 2. 🧪 Testar a aplicação localmente
Execute o script de teste com cobertura consolidada:

#### 🖥️ Windows (PowerShell):

```powershell
.\Run.ps1 -RunMode Test -ViewReport
```

#### 🐧 Linux / 🍎 macOS (Bash/Zsh):

```bash
chmod +x run.sh
./run.sh Test -view
```

Esse comando realiza:
- ✔️ Restauração de ferramentas
- 🧪 Execução dos testes unitários de todos os módulos via targets MSBuild
- 📊 Geração do relatório de cobertura (coverage.xml)
- 🌐 Criação do relatório HTML + badge de cobertura
- 👀 Abertura automática do relatório no navegador para visualização

- Dica: se quiser executar os testes sem abrir o navegador, basta omitir o parâmetro -ViewReport.

#### 🖥️ Windows (PowerShell):

```powershell
PS C:\repo\CBDCalc> .\Run.ps1 -RunMode Test
```

#### 🐧 Linux / 🍎 macOS (Bash/Zsh):
```bash
./run.sh Test 
```

### 3. 🚀 Executar a aplicação (Ambiente Docker)

A aplicação foi estruturada para ser executada exclusivamente através do **Docker Compose**, garantindo consistência, isolamento e portabilidade de todo o ambiente, inclusive microserviços e gateways internos.
Entretanto no windows é possível utilizar o visual studio, porém é necessário executar nennhum dos perfis de execução disponiveis, selecionando multiplos projetos de execução, `CDBCalculator` e `CDBCalculator.Server`.
Para inicializar o ambiente completo:

#### 🖥️ Windows (PowerShell):

```powershell
.\Run.ps1 -RunMode App
```

#### 🐧 Linux / 🍎 macOS (Bash/Zsh):
```bash
./run.sh App 
```

Esse comando dispara:
- 🐳 Subida dos serviços via docker-compose
- ⚙️ Inicialização dos backends e gateways
- 🌐 Exposição de endpoints para testes e integração

Apos isso abra o navegador em [http://localhost:5000](http://localhost:5000) para acessar a aplicação.

### 📊 Cobertura de Testes

Cobertura de Testes é gerada automaticamente durante a execução dos testes unitários. O relatório é consolidado e disponibilizado em HTML, no github pages: [CDBCalc Report](https://weslleymurdock.github.io/CDBCalc/).

## 📄 Licença
MIT — consulte o arquivo LICENSE para detalhes.

