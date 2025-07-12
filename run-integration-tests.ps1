# Script para executar testes integrados
# Este script inicia o ambiente de teste, executa os testes e limpa os recursos

param(
    [switch]$SkipCleanup
)

Write-Host "Iniciando execucao dos testes integrados..." -ForegroundColor Green

# Funcao para verificar se o Docker esta rodando
function Test-DockerRunning {
    try {
        docker version | Out-Null
        return $true
    }
    catch {
        return $false
    }
}

# Funcao para aguardar servicos ficarem prontos
function Wait-ForServices {
    Write-Host "Aguardando servicos ficarem prontos..." -ForegroundColor Yellow
    
    # Aguardar MongoDB
    $maxAttempts = 30
    $attempt = 0
    do {
        $attempt++
        try {
            $result = Invoke-RestMethod -Uri "http://localhost:27017" -Method GET -TimeoutSec 5 -ErrorAction Stop
            Write-Host "MongoDB esta pronto!" -ForegroundColor Green
            break
        }
        catch {
            if ($attempt -ge $maxAttempts) {
                Write-Host "Timeout aguardando MongoDB ficar pronto" -ForegroundColor Red
                exit 1
            }
            Write-Host "Aguardando MongoDB... (tentativa $attempt/$maxAttempts)" -ForegroundColor Yellow
            Start-Sleep -Seconds 2
        }
    } while ($true)
    
    # Aguardar RabbitMQ
    $attempt = 0
    do {
        $attempt++
        try {
            $result = Invoke-RestMethod -Uri "http://localhost:15672/" -Method GET -TimeoutSec 5 -ErrorAction Stop
            Write-Host "RabbitMQ esta pronto!" -ForegroundColor Green
            break
        }
        catch {
            if ($attempt -ge $maxAttempts) {
                Write-Host "Timeout aguardando RabbitMQ ficar pronto" -ForegroundColor Red
                exit 1
            }
            Write-Host "Aguardando RabbitMQ... (tentativa $attempt/$maxAttempts)" -ForegroundColor Yellow
            Start-Sleep -Seconds 2
        }
    } while ($true)
}

# Funcao para limpar recursos
function Cleanup-Resources {
    Write-Host "Limpando recursos..." -ForegroundColor Yellow
    docker-compose -f docker-compose.test.yml down -v
    Write-Host "Recursos limpos!" -ForegroundColor Green
}

# Verificar se Docker esta rodando
if (-not (Test-DockerRunning)) {
    Write-Host "Docker nao esta rodando. Por favor, inicie o Docker Desktop." -ForegroundColor Red
    exit 1
}

# Parar containers existentes se houver
Write-Host "Parando containers existentes..." -ForegroundColor Yellow
docker-compose -f docker-compose.test.yml down -v 2>$null

# Iniciar ambiente de teste
Write-Host "Iniciando ambiente de teste..." -ForegroundColor Yellow
docker-compose -f docker-compose.test.yml up -d

# Aguardar servicos ficarem prontos
Wait-ForServices

# Executar testes integrados
Write-Host "Executando testes integrados..." -ForegroundColor Yellow
try {
    dotnet test ServiceAuth.IntegratedTests/ServiceAuth.IntegratedTests.csproj --logger "console;verbosity=detailed"
    $testExitCode = $LASTEXITCODE
    
    if ($testExitCode -eq 0) {
        Write-Host "Todos os testes passaram!" -ForegroundColor Green
    } else {
        Write-Host "Alguns testes falharam!" -ForegroundColor Red
    }
}
catch {
    Write-Host "Erro ao executar testes: $($_.Exception.Message)" -ForegroundColor Red
    $testExitCode = 1
}

# Limpar recursos se nao foi solicitado para pular
if (-not $SkipCleanup) {
    Cleanup-Resources
} else {
    Write-Host "Containers mantidos ativos (use -SkipCleanup para manter)" -ForegroundColor Cyan
}

# Retornar codigo de saida dos testes
exit $testExitCode 