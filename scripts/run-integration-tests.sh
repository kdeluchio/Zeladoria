#!/bin/bash

# Script para executar testes integrados
# Este script inicia o ambiente de teste, executa os testes e limpa os recursos

SKIP_CLEANUP=false

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --skip-cleanup)
            SKIP_CLEANUP=true
            shift
            ;;
        *)
            echo "Opção desconhecida: $1"
            echo "Uso: $0 [--skip-cleanup]"
            exit 1
            ;;
    esac
done

echo "🚀 Iniciando execução dos testes integrados..."

# Função para verificar se o Docker está rodando
check_docker() {
    if ! docker version >/dev/null 2>&1; then
        echo "❌ Docker não está rodando. Por favor, inicie o Docker Desktop."
        exit 1
    fi
}


# Função para aguardar serviços ficarem prontos
wait_for_services() {
    echo "⏳ Aguardando serviços ficarem prontos..."
    
    # Aguardar MongoDB
    echo "⏳ Aguardando MongoDB..."
    local max_attempts=30
    local attempt=0
    
    while [ $attempt -lt $max_attempts ]; do
        attempt=$((attempt + 1))
        if docker exec mongodb-test mongosh --eval "db.adminCommand('ping')" >/dev/null 2>&1; then
            echo "✅ MongoDB está pronto!"
            break
        fi
        
        if [ $attempt -eq $max_attempts ]; then
            echo "❌ Timeout aguardando MongoDB ficar pronto"
            exit 1
        fi
        
        echo "⏳ Aguardando MongoDB... (tentativa $attempt/$max_attempts)"
        sleep 2
    done
    
    # Aguardar RabbitMQ
    echo "⏳ Aguardando RabbitMQ..."
    attempt=0
    
    while [ $attempt -lt $max_attempts ]; do
        attempt=$((attempt + 1))
        if docker exec rabbitmq-test rabbitmq-diagnostics ping >/dev/null 2>&1; then
            echo "✅ RabbitMQ está pronto!"
            break
        fi
        
        if [ $attempt -eq $max_attempts ]; then
            echo "❌ Timeout aguardando RabbitMQ ficar pronto"
            exit 1
        fi
        
        echo "⏳ Aguardando RabbitMQ... (tentativa $attempt/$max_attempts)"
        sleep 2
    done
}

# Função para limpar recursos
cleanup_resources() {
    echo "🧹 Limpando recursos..."
    docker-compose -f docker-compose.test.yml down -v
    echo "✅ Recursos limpos!"
}

# Verificar se Docker está rodando
check_docker

# Parar containers existentes se houver
echo "🛑 Parando containers existentes..."
docker-compose -f docker-compose.test.yml down -v 2>/dev/null || true

# Iniciar ambiente de teste
echo "🐳 Iniciando ambiente de teste..."
docker-compose -f docker-compose.test.yml up -d

# Aguardar serviços ficarem prontos
wait_for_services

# Executar testes unitários
echo "🧪 Executando testes unitários..."
if dotnet test ServiceNotification.UnitTests/ServiceNotification.UnitTests.csproj --logger "console;verbosity=detailed"; then
    echo "✅ Todos os testes unitários passaram!"
    unit_test_exit_code=0
else
    echo "❌ Alguns testes unitários falharam!"
    unit_test_exit_code=1
fi

# Executar testes integrados
echo "🧪 Executando testes integrados..."
if dotnet test ServiceAuth.IntegratedTests/ServiceAuth.IntegratedTests.csproj --logger "console;verbosity=detailed"; then
    echo "✅ Todos os testes integrados passaram!"
    integration_test_exit_code=0
else
    echo "❌ Alguns testes integrados falharam!"
    integration_test_exit_code=1
fi

# Determinar código de saída final
if [ $unit_test_exit_code -eq 0 ] && [ $integration_test_exit_code -eq 0 ]; then
    test_exit_code=0
    echo "✅ Todos os testes passaram!"
else
    test_exit_code=1
    echo "❌ Alguns testes falharam!"
fi

# Limpar recursos se não foi solicitado para pular
if [ "$SKIP_CLEANUP" = false ]; then
    cleanup_resources
else
    echo "ℹ️  Containers mantidos ativos (use --skip-cleanup para manter)"
fi

# Retornar código de saída dos testes
exit $test_exit_code 