# ServiceAuth

Serviço de autenticação responsável por gerenciar usuários e autenticação JWT.

## Funcionalidades

- **Login**: Autenticação de usuários com JWT
- **Signup**: Cadastro de novos usuários
- **Forgot Password**: Solicitação de reset de senha via RabbitMQ
- **Reset Password**: Reset de senha com token

## Tecnologias

- `.NET 8`
- `Docker`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `System.IdentityModel.Tokens.Jwt`
- `MongoDB.Driver`
- `RabbitMQ`
- `FluentValidation.AspNetCore`
- `Swashbuckle.AspNetCore`

## Pré-requisitos

- [Docker](https://www.docker.com/)

## Instalação

```bash
git clone https://github.com/kdeluchio/Zeladoria.git 
cd Zeladoria
docker-compose up --build -d 
```

## RabbitMQ

O serviço utiliza RabbitMQ para enviar mensagens de reset de senha. 
Acesse o RabbitMQ UI `http://localhost:5672` para monitorar as mensagens produzidas.
```
 "UserName": "guest",
 "Password": "guest",
```

### Queue

É enviado mensagens para a fila `forgot-password` do RabbitMQ toda vez que o usuário esqueceu a senha e geramos um token que é enviado via e-mail.
- Corpo da mensagem:

```json
{
  "Email": "usuario@exemplo.com",
  "Token": "token-de-reset-gerado"
}
```

## Teste a API

### Swagger UI

Acesse o Swagger UI em `http://localhost:5021/swagger` para testar a API com interface gráfica. Nenhuma rota desse serviço é necessário possuir autenticação.
