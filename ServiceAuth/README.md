# ServiceAuth

Serviço de autenticação responsável por gerenciar usuários e autenticação JWT.

## Funcionalidades

- **Login**: Autenticação de usuários com JWT
- **Signup**: Cadastro de novos usuários
- **Forgot Password**: Solicitação de reset de senha via RabbitMQ
- **Reset Password**: Reset de senha com token
- **Get User**: Consulta de usuário por ID

## Configuração

### RabbitMQ

O serviço utiliza RabbitMQ para enviar mensagens de reset de senha. As configurações estão em `appsettings.json`:

```json
{
  "RabbitMQSettings": {
    "HostName": "rabbitmq",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/"
  }
}
```

### QueueProducerService

A classe `QueueProducerService` é responsável por enviar mensagens para a fila `forgot-password` do RabbitMQ.

#### Uso

```csharp
// Injeção de dependência
public class AuthService
{
    private readonly IQueueProducerService _producer;
    
    public AuthService(IQueueProducerService producer)
    {
        _producer = producer;
    }
    
    public async Task ForgotPasswordAsync(string email)
    {
        // ... lógica de negócio ...
        
        // Envia mensagem para a fila
        await _producer.SendAsync(new QueueMessage(email, resetToken));
    }
}
```

#### Formato da Mensagem

A mensagem enviada para a fila `forgot-password` tem o seguinte formato:

```json
{
  "Email": "usuario@exemplo.com",
  "Token": "token-de-reset-gerado"
}
```

## Endpoints

### POST /auth/login
Autentica um usuário e retorna um token JWT.

### POST /auth/signup
Cadastra um novo usuário.

### POST /auth/forgot-password
Solicita reset de senha. Envia mensagem para a fila `forgot-password`.

### POST /auth/reset-password
Reseta a senha usando o token recebido.

### GET /auth/user/{id}
Consulta um usuário por ID.

## Execução

```bash
# Desenvolvimento local
dotnet run

# Docker
docker-compose up serviceauthapi
```

## Dependências

- MongoDB: Armazenamento de usuários
- RabbitMQ: Envio de mensagens de reset de senha
- JWT: Autenticação e autorização

## Arquitetura

```
AuthService
    ↓
QueueProducerService (RabbitMQ)
    ↓
Fila: forgot-password
    ↓
ServiceNotification (Consumidor)
```
