# ServiceNotification

Serviço responsável por processar notificações através de filas RabbitMQ.

## Tecnologias

- `.NET 8`
- `Worker`
- `Docker`
- `RabbitMQ`


## Pré-requisitos

- [Docker](https://www.docker.com/)

## Instalação

```bash
git clone https://github.com/kdeluchio/Zeladoria.git 
cd Zeladoria
docker-compose up --build -d 
```


## RabbitMQ

O serviço utiliza RabbitMQ para receber mensagens de reset de senha. 
Acesse o RabbitMQ UI `http://localhost:5672` para monitorar as mensagens consumidas.
```
 "UserName": "guest",
 "Password": "guest",
```

### Queue

Lêmos da fila `forgot-password` do RabbitMQ toda vez que o serviceAuth produz uma mensagem e a processamos.
- Corpo da mensagem:

```json
{
  "Email": "usuario@exemplo.com",
  "Token": "token-de-reset-gerado"
}
```

## Teste do Serviço

Acesse o container serviceNotification e monitore os logs para verificar se houve o processamento das mensagens da fila.

### Logs

O serviço registra logs detalhados para:
- Início e parada do serviço
- Recebimento de mensagens
- Processamento de cada tipo de mensagem
- Erros durante o processamento
