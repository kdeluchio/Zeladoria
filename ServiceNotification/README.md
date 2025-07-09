# ServiceNotification

Serviço responsável por processar notificações através de filas RabbitMQ.

## Estrutura do Projeto

### Services/
- **QueueConsumerService**: Gerencia a conexão e consumo da fila RabbitMQ
- **MessageProcessorService**: Processa diferentes tipos de mensagens de forma organizada

### Worker.cs
- Serviço hospedado que coordena o funcionamento do sistema
- Utiliza os serviços especializados para manter o código organizado

## Configuração

O serviço utiliza as seguintes configurações no `appsettings.json`:

```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest",
    "QueueName": "reset-password"
  }
}
```

## Tipos de Mensagens Suportadas

O `MessageProcessorService` processa os seguintes tipos de mensagens:

### 1. Reset Password (`reset-password`)
- Processa solicitações de reset de senha
- Envia emails com links de reset

### 2. Order Created (`order-created`)
- Processa notificações de novas ordens criadas
- Notifica técnicos disponíveis

### 3. Service Assigned (`service-assigned`)
- Processa notificações de serviços atribuídos
- Notifica clientes sobre atribuição de técnicos

### 4. Unknown Messages
- Processa mensagens de tipos não reconhecidos
- Registra logs para análise posterior

## Formato das Mensagens

As mensagens devem ser enviadas em formato JSON:

```json
{
  "type": "reset-password",
  "content": "Mensagem opcional",
  "data": {
    "userId": "123",
    "email": "user@example.com"
  }
}
```

## Execução

```bash
dotnet run
```

## Logs

O serviço registra logs detalhados para:
- Início e parada do serviço
- Recebimento de mensagens
- Processamento de cada tipo de mensagem
- Erros durante o processamento

## Arquitetura

```
Worker (BackgroundService)
    ↓
QueueConsumerService (Gerencia RabbitMQ)
    ↓
MessageProcessorService (Processa mensagens)
```

Esta estrutura permite:
- Separação clara de responsabilidades
- Fácil manutenção e extensão
- Testabilidade individual dos componentes
- Organização do código por funcionalidade 