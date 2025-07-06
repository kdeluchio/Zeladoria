# ServiceAuth - API de Autenticação

Este microserviço é responsável pela autenticação e autorização de usuários, fornecendo funcionalidades de login, cadastro, reset de senha e geração de tokens JWT.

## Funcionalidades

- **Login**: Autenticação de usuários com email e senha
- **Signup**: Cadastro de novos usuários
- **Forgot Password**: Solicitação de reset de senha
- **Reset Password**: Redefinição de senha com token
- **Get User**: Obtenção de dados do usuário por ID

## Tecnologias

- .NET 8
- MongoDB
- JWT (JSON Web Tokens)
- FluentValidation
- Minimal APIs

## Configuração

### MongoDB
Certifique-se de que o MongoDB está rodando na porta 27017 ou atualize a string de conexão no `appsettings.json`.

### JWT Settings
Configure as seguintes propriedades no `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "sua-chave-secreta-com-pelo-menos-32-caracteres",
    "Issuer": "ServiceAuth",
    "Audience": "ServiceAuth",
    "ExpirationHours": "24"
  }
}
```
