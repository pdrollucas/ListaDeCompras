# ListaDeCompras - Gerenciador de Listas

**PAC - Projeto de Aprendizagem Colaborativa Extensionista do Curso de Engenharia de Software da Católica de Santa Catarina**

**Alunos**: Eduardo Correa, Eduardo Fritz e Pedro Lucas Luckow

**Professores orientadores:** Luiz Carlos Camargo e Claudinei Dias

## Justificativa

O desenvolvimento do Gerenciador de Listas se justifica pela necessidade apresentada por um cliente real que buscava uma forma prática de organizar listas para uso doméstico. A ausência de uma solução personalizada motivou a criação de um sistema capaz de atender exatamente às suas demandas. Além disso, o projeto permitiu aplicar tecnologias relevantes do mercado, como Vue.js, C# e MySQL. A proposta também contribuiu para o aprofundamento de conhecimentos em desenvolvimento web e boas práticas de design. Dessa forma, o trabalho une utilidade prática e valor acadêmico.

## Descrição

O Gerenciador de Listas é uma aplicação web mobile desenvolvida para permitir que usuários criem e organizem listas personalizadas contendo itens com nome e quantidade, seja numérica ou em unidades de medida. O sistema inclui funcionalidades de cadastro, login, recuperação de senha via e-mail e gerenciamento completo de listas. Construído com Vue.js no front-end, C# no back-end e MySQL como banco de dados, o sistema oferece uma interface simples, responsiva e focada na usabilidade. Seu objetivo é fornecer uma ferramenta prática para organização doméstica e atender às necessidades específicas de um cliente real.


## Tecnologias Utilizadas

- **Backend**: C#
- **Frontend**: Vue.js
- **Banco de dados:**: MySQL
- **Design**: Figma
- **Gestão do Projeto**: Trello

## Principais telas desenvolvidas
<img width="746" height="776" alt="Captura de tela 2025-11-30 180556" src="https://github.com/user-attachments/assets/2c4b44a3-c133-4e87-b343-ffb3966e4ed4" />


## Link para o Design completo

- [Figma - Loja de Compras](https://www.figma.com/design/OLKqfrTJXXXY2XhvWcTTrv/Untitled?node-id=0-1&t=LtCk0v5J5SQsCtRT-1)

<hr/>

# Configuração
## Requisitos para Desenvolvimento

- **Node.js**: versão 20.x ou superior
- **npm** (ou **yarn/pnpm**) para gerenciamento de pacotes do front-end
- **.NET SDK** compatível com o backend em C# (ex.: .NET 8.0 ou versão definida no projeto backend)

## Ambiente de Desenvolvimento

### 1. Clonar o repositório

```bash
git clone <URL_DO_REPOSITORIO>
cd ListaDeCompras
```

### 2. Configurar e rodar o Front-end (Vue + Vite)

```bash
cd front-end
npm install
npm run dev
```

Por padrão, o Vite irá iniciar o servidor de desenvolvimento em algo como `http://localhost:5173` (ou porta exibida no terminal).

### 3. Configurar e rodar o Backend (C#)

Alterar o arquivo appsettings.json.example para appsettings.json e inserir as configurações necessárias do seu banco de dados

Na pasta `backend`, execute o projeto C# (ajuste o comando conforme a estrutura do seu projeto):

```bash
cd ../backend
dotnet restore
dotnet run
```

O backend será iniciado na porta configurada no projeto (por exemplo, `http://localhost:5000` ou `http://localhost:8080`).

### 4. Fluxo de desenvolvimento sugerido

- Inicie primeiro o backend (`dotnet run`).
- Em seguida, rode o front-end (`npm run dev`).
- Acesse o endereço do front-end no navegador.

----

## Documentação

Para mais informações, consultar o arquivo de documentação:
[documentacao_gerenciador-de-listas.pdf](https://github.com/user-attachments/files/23842598/documentacao_gerenciador-de-listas.pdf)



# Arquivo documentação

[documentacao-gerenciador-de-listas.pdf](https://github.com/user-attachments/files/22182579/documentacao-gerenciador-de-listas.pdf)



