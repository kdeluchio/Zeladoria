# Teste da Funcionalidade QueueProducerService

Este documento descreve como testar a funcionalidade de envio de mensagens para a fila `forgot-password` do RabbitMQ.

## Pré-requisitos

1. Docker e Docker Compose instalados
2. Todos os serviços rodando: `docker-compose up -d`

## Teste da Funcionalidade

### 1. Verificar se os serviços estão rodando

```bash
docker-compose ps
```

Você deve ver:
- mongodb
- rabbitmq
- serviceauthapi
- servicenotification

### 2. Acessar o RabbitMQ Management

Abra o navegador e acesse: `http://localhost:15672`
- Usuário: `guest`
- Senha: `guest`

Na interface do RabbitMQ, você pode:
- Verificar se a fila `forgot-password` foi criada
- Monitorar mensagens sendo enviadas e consumidas

### 3. Testar o endpoint de forgot-password

```bash
# Primeiro, crie um usuário
curl -X POST http://localhost:5021/auth/signup \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Usuário Teste",
    "email": "teste@exemplo.com",
    "password": "senha123",
    "cpf": "12345678901",
    "phone": "11999999999"
  }'

# Depois, solicite o reset de senha
curl -X POST http://localhost:5021/auth/forgot-password \
  -H "Content-Type: application/json" \
  -d '"teste@exemplo.com"'
```

### 4. Verificar os logs

```bash
# Logs do ServiceAuth
docker-compose logs serviceauthapi

# Logs do ServiceNotification
docker-compose logs servicenotification
```

### 5. Verificar a fila no RabbitMQ

Na interface do RabbitMQ Management:
1. Vá para a aba "Queues"
2. Clique na fila `forgot-password`
3. Verifique se há mensagens na fila
4. Monitore o consumo das mensagens pelo ServiceNotification

## Estrutura da Mensagem

A mensagem enviada para a fila tem o seguinte formato JSON:

```json
{
  "Email": "teste@exemplo.com",
  "Token": "abc123def456..."
}
```

## Fluxo Completo

1. **Usuário solicita reset de senha** → POST `/auth/forgot-password`
2. **AuthService gera token** → Salva no banco de dados
3. **QueueProducerService envia mensagem** → Fila `forgot-password`
4. **ServiceNotification consome mensagem** → Processa e envia email
5. **Usuário recebe email** → Com link para reset de senha

## Troubleshooting

### Problema: Fila não é criada
- Verifique se o RabbitMQ está rodando: `docker-compose logs rabbitmq`
- Verifique as configurações no `appsettings.json`

### Problema: Mensagem não é enviada
- Verifique os logs do ServiceAuth
- Verifique a conexão com RabbitMQ
- Verifique se o `QueueProducerService` está registrado corretamente

### Problema: Mensagem não é consumida
- Verifique se o ServiceNotification está rodando
- Verifique os logs do ServiceNotification
- Verifique se o nome da fila está correto nas configurações

## Comandos Úteis

```bash
# Reiniciar todos os serviços
docker-compose down
docker-compose up -d

# Ver logs em tempo real
docker-compose logs -f serviceauthapi
docker-compose logs -f servicenotification

# Verificar status dos containers
docker-compose ps

# Acessar container do RabbitMQ
docker exec -it rabbitmq bash
``` 