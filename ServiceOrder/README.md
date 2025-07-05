# ServiceOrder API

API para gerenciamento de ordens de serviço usando .NET 8 Minimum API e MongoDB.

## Pré-requisitos

- .NET 8 SDK
- MongoDB (local ou remoto)

## Configuração

1. **MongoDB**: Certifique-se de que o MongoDB está rodando localmente na porta 27017 ou atualize a string de conexão no `appsettings.json`.

2. **Configuração do banco**: As configurações do MongoDB estão no arquivo `appsettings.json`:
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "ServiceOrderDb"
  }
}
```

## Executando o projeto

```bash
dotnet run
```

A API estará disponível em `https://localhost:7000` (ou a porta configurada).
