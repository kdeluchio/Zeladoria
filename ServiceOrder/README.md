# ServiceOrder API

Este projeto implementa a API de ordens de serviço e cadastro de serviços com autenticação JWT obrigatória.

## Funcionalidades

- **Order**: Cadastro das ordens de serviços
- **Service**: Cadastro dos serviços prestados

## Tecnologias

- `.NET 8`
- `Docker`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `System.IdentityModel.Tokens.Jwt`
- `MongoDB.Driver`
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

## Teste a API

### Autenticação JWT

Todas as requisições HTTP para este serviço devem incluir um token JWT válido gerado pelo ServiceAuth.

### Como obter um token JWT

1. Faça login no ServiceAuth:
```bash
POST /auth/login
{
    "email": "seu@email.com",
    "password": "suasenha"
}
```

2. Use o token retornado no header Authorization:
```
Authorization: Bearer {seu_token_jwt}
```

### Swagger UI

Acesse o Swagger UI em `http://localhost:5011/swagger` para testar a API com interface gráfica. O Swagger está configurado para aceitar tokens JWT através do botão "Authorize".

### Informações do Usuário

O token JWT contém as seguintes informações do usuário que podem ser acessadas nos endpoints:

- **UserId**: ID único do usuário
- **UserName**: Nome do usuário
- **UserEmail**: Email do usuário
- **UserRole**: Role/perfil do usuário
