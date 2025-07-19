# Zeladoria

Sistema de zeladoria urbana onde o cidadão pode apontar problemas e sugestões para seu bairro e cidade. Baseado nos dados colhidos, o sistema ajudará na melhoria do planejamento urbano e na prestação de serviços efetuados pelas prefeituras ou empresas contratadas.

## Funcionalidades 
### Entregues
- Web site backoffice 
	- Controle de ordens de serviço 
	- Autenticação de usuários
	- Mensageria para comunicação com os usuários

### Futuras
- App mobile 
	- Reportar problemas de zeladoria de maneira rápida, fácil e centralizada 
	- Sugestão de melhorias para a comunidade 
	- Feedback do status das demandas 
	- Geolocalização 
	- Avaliar serviços 
	- Visualização de eventos na região 

- Web site backoffice 
	- Análise de dados e inteligência artificial para gerar insights e previsões de cenários 
	- Mapa de calor para gerenciamento dos problemas 

- Apis web 
	- Acesso a dados não sigilosos 
	- Dados de interesse de demandas de comercio por região “radar de oportunidades” 
	- Dados de segurança pública por região 


## Detalhes dos Serviços

- [ServiceAuth/README.md](ServiceAuth/README.md)
- [ServiceOrder/README.md](ServiceOrder/README.md)
- [ServiceNotification/README.md](ServiceNotification/README.md)

## Configurações Gerais

### Pipeline CI
O pipeline de Integração Contínua (CI) é definido no arquivo `.github/workflows/integration-tests.yml` e é responsável por garantir a qualidade do código a cada alteração enviada para o repositório. Ele é disparado automaticamente em pushes e pull requests para a branch `main`, além de poder ser executado manualmente.

**Principais etapas do CI:**
- Checkout do código do repositório.
- Configuração do ambiente .NET 8.
- Restauro e cache de dependências.
- Verificação do Docker e Docker Compose.
- Subida do ambiente de testes com `docker-compose.test.yml` (MongoDB, RabbitMQ e serviços necessários).
- Aguarda os serviços ficarem prontos (MongoDB e RabbitMQ).
- Execução dos testes:
  - Testes unitários do ServiceNotification.
  - Testes integrados do ServiceAuth.
  - Testes integrados do ServiceOrder.
- Limpeza dos recursos ao final (derruba containers e remove volumes, salvo se solicitado manter para debug).
- Coleta e upload de logs dos containers em caso de falha.

Além disso, existe o script `scripts/run-integration-tests.sh` que permite executar localmente o mesmo fluxo de testes integrados, facilitando a validação antes de enviar código para o repositório.

### Pipeline CD
O pipeline de Entrega Contínua (CD) está definido no arquivo `.github/workflows/docker-publish.yml` e é responsável por construir e publicar as imagens Docker dos serviços após a aprovação dos testes.

**Funcionamento do CD:**
- Disparado automaticamente quando o workflow de testes (`Testes Unitários e Integrados`) é concluído com sucesso.
- Para cada serviço (`ServiceAuth`, `ServiceNotification`, `ServiceOrder`):
  - Faz checkout do código.
  - Prepara o ambiente de build Docker.
  - Realiza login no DockerHub usando segredos do repositório.
  - Constrói a imagem Docker do serviço correspondente.
  - Publica a imagem no DockerHub com a tag `latest`.

Dessa forma, sempre que uma alteração passa pelos testes, as imagens atualizadas dos serviços são disponibilizadas automaticamente no DockerHub, prontas para serem implantadas em ambientes de homologação ou produção.
