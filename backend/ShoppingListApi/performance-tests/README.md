# Testes de Carga - Lista de Compras API

Este diretório contém os testes de carga usando k6 para a API de Lista de Compras.

## Pré-requisitos

1. Instale o k6: https://k6.io/docs/get-started/installation/
2. Certifique-se que a API esteja rodando em `http://localhost:5139`

## Configuração do Teste

O teste de carga simula o seguinte cenário:

1. Rampa inicial: 0 → 500 usuários em 30 segundos
2. Aumento: 500 → 1000 usuários em 30 segundos
3. Carga máxima: 1000 usuários por 1 minuto
4. Rampa de descida: 1000 → 0 em 30 segundos

### Métricas Monitoradas

- Taxa de erro: deve ser menor que 10%
- Tempo de resposta:
  - 95% das requisições < 2 segundos
  - 99% das requisições < 3 segundos

## Como executar os testes

1. Abra um terminal na pasta `performance-tests`
2. Execute o comando:
   ```bash
   k6 run load-test.js
   ```

## Configuração dos Testes

O teste está configurado com os seguintes estágios:
1. Rampa de subida: 0 → 20 usuários em 30 segundos
2. Carga constante: 20 usuários por 1 minuto
3. Rampa de descida: 20 → 0 usuários em 30 segundos

### Thresholds (Critérios de Sucesso)
- Taxa de erro deve ser menor que 10%
- 95% das requisições devem completar em menos de 1 segundo

## Cenários de Teste

O script de teste inclui os seguintes cenários:

1. Login de usuário
   - Faz login com credenciais de teste
   - Extrai o token JWT da resposta

2. Listagem de Itens
   - Usa o token JWT obtido no login
   - Faz uma requisição GET para obter todos os itens

3. Criação de Item
   - Cria um novo item com dados aleatórios
   - Verifica se a criação foi bem-sucedida

## Notas

- Antes de executar os testes, certifique-se de que existe um usuário com as credenciais:
  - Email: test@example.com
  - Senha: Test@123
- Os resultados dos testes serão exibidos no terminal após a execução
- Para modificar os parâmetros de teste, edite as configurações no arquivo `load-test.js`
