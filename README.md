# iti Digital Backend Challenge

Api para cumprir o desafio de criar uma API robusta que possua inicialmente um endpoint para validação de senha.

## Critério de validação

Uma senha é considerada válida quando a mesma possuir as seguintes definições:

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

## Premissas consideradas

Tendo como base que a validação senha é algo bem simples ea princípio pode ser resolvido com uma expressão regular e poucas linhas de código, contudo o levou-se em consideração o seguinte:

* Esta api poderá estar inserida em um ecosistema maior que precise de escalabilidade e disponibilidade, e isso motivou a implementação de uma estrutura de projeto (pattern) mais robusta.
* A necessidade de estar preparada para receber novas features/funcionalidades sem degradar desempenho. E isso também justifica a decisão do pattern escolhido (Clean Architeture/CQRS)
* Que as regras de validação da senha é algo que possa ser modificado de forma rápida e por isso as mesmas foram colocadas no arquivo de configuração (appsettings) e com um provider fornecendo sua leitura, permitindo uma rápida e fácil migração para outro repositório de armazenamento (base de dados, variáveis de ambientes, serviços de terceiros como split.io, etc).

## Abordagem e Escolha da Stack Tecnológica

TODO: continue

## Problema

Construa uma aplicação que exponha uma api web que valide se uma senha é válida.

Input: Uma senha (string).  
Output: Um boolean indicando se a senha é válida.

Embora nossas aplicações sejam escritas em Kotlin e C# (.net core), você não precisa escrever sua solução usando elas. Use a linguagem de programação que considera ter mais conhecimento.

## Pontos que daremos maior atenção

- Testes de unidade / integração
- Abstração, acoplamento, extensibilidade e coesão
- Design de API
- Clean Code
- SOLID
- Documentação da solução no *README* 

## Pontos que não iremos avaliar

- docker file
- scripts ci/cd
- coleções do postman ou ferramentas para execução

### Sobre a documentação

Nesta etapa do processo seletivo queremos entender as decisões por trás do código, portanto é fundamental que o *README* tenha algumas informações referentes a sua solução.

Algumas dicas do que esperamos ver são:

- Instruções básicas de como executar o projeto;
- Detalhes sobre a sua solução, gostariamos de saber qual foi seu racional nas decisões;
- Caso algo não esteja claro e você precisou assumir alguma premissa, quais foram e o que te motivou a tomar essas decisões.