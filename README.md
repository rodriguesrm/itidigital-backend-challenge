[![Build & Test](https://github.com/rodriguesrm/itidigital-backend-challenge/actions/workflows/dotnet-core.yml/badge.svg)](https://github.com/rodriguesrm/itidigital-backend-challenge/actions/workflows/dotnet-core.yml)

# **Iti Digital Backend Challenge**

Api para cumprir o desafio de criar uma API robusta que possua inicialmente um *endpoint* para validação de senha.

## **Critério de validação**

Uma senha é considerada válida quando a mesma atender as seguintes regras:

- Nove ou mais caracteres
- Não possuir caracteres repetidos dentro do conjunto
- Ao menos 1 dígito
- Ao menos 1 letra minúscula
- Ao menos 1 letra maiúscula
- Ao menos 1 caractere especial
  - Os caracteres especiais aceitos são: ! @ # $ % ^ & * ( ) - +

Exemplo:  

```c#
IsValid("") // false  
IsValid("aa") // false  
IsValid("ab") // false  
IsValid("AAAbbbCc") // false  
IsValid("AbTp9!foo") // false  
IsValid("AbTp9!foA") // false
IsValid("AbTp9 fok") // false
IsValid("AbTp9!fok") // true
```

> **_Nota:_**  Espaços em branco não são considerados como caracteres válidos.

## **Premissas consideradas**

Tendo como base que a validação senha é algo bem simples e a princípio pode ser resolvido com uma *expressão regular* e poucas linhas de código, levou-se em consideração o seguinte:

* Esta api poderá estar inserida em um ecosistema maior que precise de escalabilidade e disponibilidade, e isso motivou a implementação de uma estrutura de projeto (pattern) mais robusta.
* A necessidade de estar preparada para receber novas features/funcionalidades sem degradar desempenho. E isso também justifica a decisão do *pattern* escolhido (Clean Architeture/CQRS)
* Que as regras de validação da senha é algo que possa ser modificado de forma rápida e por isso as mesmas foram colocadas no arquivo de configuração (appsettings) e com um *provider* fornecendo sua leitura, permitindo uma rápida e fácil migração para outro repositório de armazenamento (base de dados, variáveis de ambientes, serviços de terceiros, etc).

## **Abordagem e Escolha da Stack Tecnológica**

* **Repositório de Dados das Regras de Validação:** Para tornar simples e rápido questão do repositório das regras de validação, foi criado um *provider* capturando informações do `appsettings.json` da aplicação, desta forma uma eventual migração para serviço de configuração dinâmica como [Split](https://www.split.io/) ou bases de dados, seja ela relacional ou não-relacional, é algo simples e rápido de se fazer.
* **Design de API:** Para criação da API seguiu-se o padrão de maturidade de REST. Também foi escolhido o pacote [MediatR](https://www.nuget.org/packages/MediatR/) para mediar as requisições do *client* com os *handlers*, separando assim as responsabilidades. Esse pacote é muito popular e seu funcionamento é estável e de fácil compreensão, odebedenco conceitos de CQRS.
* **Logs:** Para registro de log foi utilizado o [RSoft.Logs](https://www.nuget.org/packages/RSoft.Logs/1.1.0-rc1.6) de minha autoriza para exibição de logs no console apenas, contudo através desse pacote é possível utilizar [ELK](https://www.elastic.co/pt/what-is/elk-stack) ou [SEQ](https://datalust.co/seq), mais detalhes sobre esse pacote pode ser obtido [aqui](https://github.com/rodriguesrm/rsoft-logs).
* **Endpoint:** O proposto foi a criação de um endpoint que recebesse uma senha para validação e devolvesse um booleano como resposta, e assim foi feito. Porém, adicionalmente foi colocado um parâmetro que é recebido através da *query string* chamado *detail*. Esse parâmetro é do tipo booleano que altera o comportamento da resposta de forma que se a senha estiver inválida os motivos que levaram a esse resultado são devolvidos na resposta. Isso pode ser útil para melhor orientação do usuário final do lado do *client*.

### **Organização da Solução**

O projeto está organizado da seguinte forma:

```
 +- Iti.Backend.Challenge.WebApi
 |
 +- Iti.Backend.Challenge.Application
 |
 +- Iti.Backend.Challenge.Contract
 |
 +- Iti.Backend.Challenge.Core
 |
 +- Iti.Backend.Challenge.Provider
 |
 +- Iti.Backend.Challenge.CrossCutting
 |
 +- Iti.Backend.Challenge.Tests

```

Onde:

* `Iti.Backend.Challenge.WebApi`: É onde fica o código responsável pela exposição da API. Nenhuma regra de negócio é implementada aqui. Aqui também fica o arquivo de configuração da aplicação.
* `Iti.Backend.Challenge.Application`: Aqui ficam os *handlers* das requisições feitas a API, aqui são implementados os casos de usos.
* `Iti.Backend.Challenge.Contract`: Projeto de modelos anêmicos dos quais apenas os contratos/models são implementados.
* `Iti.Backend.Challenge.Core`: Aqui está o núcleo da aplicação, todas as regras de negócios são implementadas aqui. Esta camada que ficam as `Ports/Interfaces` que o domínio do sistema irá utilizar.
* `Iti.Backend.Challenge.Provider`: Esta camada é onde fica os *providers/repositories* da aplicação. Aqui são implementados os repositórios, providers, adapters e outros mecanismos que são utilizados pelo `core` através das `Ports/Interfaces`.
* `Iti.Backend.Challenge.CrossCutting`: Aqui é a camada que faz o cruzamento de todas as demais, que "não se conhecem" mas que dependem indiretamente entre si, como o mecanismo de Injeção de Dependência.
* `Iti.Backend.Challenge.Tests`: Esta é a camada onde são estritors todos os testes. Aqui está sendo utilizado [xUnit](https://www.nuget.org/packages/xunit/), [Moq](https://www.nuget.org/packages/Moq/) e [AutoFixture](https://www.nuget.org/packages/AutoFixture/).

## **Rotas & Contratos**

Essa api possui apenas um endpoint/rota que deve ser utilizado para chamadas de validação de senha:

```c#
POST /api/validate/password
```

Os contratos/modelos de requisição e respostas são:

*Requisição*
```c#
{
  "password": "string"
}
```

*Resposta (quando detail=`false` ou não informado)*
```c#
true|false
```

*Resposta (quando detail=`true`)*
```c#
{
  "isValid": true|false,
  "errors": [
    "string"
  ]
}
```

## **Configuração das regras de validação**

Conforme descrito acima, por conveniência no desenvolvimento das regras de validação, que são dinâmicas, podem ser alteradas. Para tanto é necessário alterar o arquivo `appsettings.json` da WebApi. O modelo da configuração deve serguir a seguinte regra:

```json
{
  "Password": {
    "ValidationRules": [
      {
        "Name": "string",
        "Regex": "string",
        "IsValidWhenMatch": true|false,
        "Message": "string"
      }
    ]
  }
}
```

Onde:
* Name: Nome da regra, deve ser uma identificação única
* Regex: Expressão regular a ser aplicada na análised da senha
* IsValidWhenMatch: Indica o comportamento da validação. Indica se a senha será válida quando o padrão da regex coincidir (`true`) ou falhar (`false`).
* Message: Mensagem a ser devolvida para o `client` quando a senha for invalidada na aplicação dessa regra

*Exemplo:*

```json
{
  "Password": {
    "ValidationRules": [
      {
        "Name": "SmallLetters",
        "Regex": "[a-z]",
        "IsValidWhenMatch": true,
        "Message": "Password must contain at least one lowercase letter"
      },
      {
        "Name": "CapitalLetters",
        "Regex": "[A-Z]",
        "IsValidWhenMatch": true,
        "Message": "Password must contain at least one capital letter"
      },
      {
        "Name": "Space",
        "Regex": "\\s",
        "IsValidWHenMatch": false,
        "Message": "Password must not contain space characters"
      }
    ]
  }
}
```

**IMPORTANTE:** As senhas vazias ou com algum caracter duplicado são consideradas inválidas. Estas são as únicas duas regras que são fixas e se houver necessidade de mudança uma alteração no código será necessária.

## **Execução usando Visual Studio**

* Faça o clone deste repositório
* Abra o projeto através do arquivo [`Iti.Backend.Challenge.sln`](src/Iti.Backend.Challenge.sln)
* Defina o projeto `Iti.Backend.Challenge.WebApi` como sendo o *start-up* da solução.
* Selecione o perfil de execução como `Kestrel`.
* Cliquem no menu `Debug` e em seguida na opção `Start Debugging` ou simplesmente pressione `F5`.
* Abra o navegador de sua preferência e acesse o endereço [http://localhost:5000](http://localhost:5000)
* Uma página do swagger com a documentação da api será apresentada e com opção de realização de chamadas de testes (`try out`)

## **Execução via linha de comando**

* Faça o clone deste repositório
* Abra um terminal de sua preferência posicionado na pasta do repositório.
* Acesse a pasta `src` contida na raiz do repositório.
* Execute o comando `dotnet run --project Iti.Backend.Challenge.WebApi`
* Abra o navegador de sua preferência e acesse o endereço [http://localhost:5000](http://localhost:5000)
* Uma página do swagger com a documentação da api será apresentada e com opção de realização de chamadas de testes (`try out`)

Bom, isso é tudo.
Qualquer dúvida, estou à disposição!
