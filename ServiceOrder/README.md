# ServiceOrder API

Este projeto implementa a API de pedidos e serviços com autenticação JWT obrigatória.

## Autenticação JWT

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

### Configuração

O projeto está configurado para validar tokens JWT com as seguintes configurações:

- **SecretKey**: `fiap-secret-key-for-jwt-signing`
- **Issuer**: `ServiceAuth`
- **Audience**: `ServiceAuth`
- **ExpirationHours**: `24`

### Endpoints Protegidos

Todos os endpoints requerem autenticação JWT:

#### Orders
- `GET /order` - Listar todos os pedidos
- `GET /order/{id}` - Obter pedido por ID
- `POST /order` - Criar novo pedido
- `PUT /order/{id}` - Atualizar pedido
- `DELETE /order/{id}` - Deletar pedido
- `GET /order/test-auth` - Teste de autenticação

#### Services
- `GET /service` - Listar todos os serviços
- `GET /service/{id}` - Obter serviço por ID
- `POST /service` - Criar novo serviço
- `PUT /service/{id}` - Atualizar serviço
- `DELETE /service/{id}` - Deletar serviço

### Testando a API

1. **Sem token (deve retornar 401 Unauthorized)**:
```bash
curl -X GET http://localhost:5001/order
```

2. **Com token válido**:
```bash
curl -X GET http://localhost:5001/order \
  -H "Authorization: Bearer {seu_token_jwt}"
```

3. **Teste de autenticação**:
```bash
curl -X GET http://localhost:5001/order/test-auth \
  -H "Authorization: Bearer {seu_token_jwt}"
```

### Swagger UI

Acesse o Swagger UI em `http://localhost:5001/swagger` para testar a API com interface gráfica. O Swagger está configurado para aceitar tokens JWT através do botão "Authorize".

### Informações do Usuário

O token JWT contém as seguintes informações do usuário que podem ser acessadas nos endpoints:

- **UserId**: ID único do usuário
- **UserName**: Nome do usuário
- **UserEmail**: Email do usuário
- **UserRole**: Role/perfil do usuário

### Dependências

- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `System.IdentityModel.Tokens.Jwt`
- `MongoDB.Driver`
- `FluentValidation.AspNetCore`
- `Swashbuckle.AspNetCore`
