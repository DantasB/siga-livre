# Siga-Livre
Essa é uma API simples que pega todas as disciplinas abertas na Universidade Federal do Rio de Janeiro. Versão em [inglês](README.pt.md).

## Content:
* [Como isso funciona?](#Como-isso-funciona?)
* [Como eu uso isso?](#Como-eu-uso-isso?)
* [Como eu posso te ajudar? ](#Como-eu-posso-te-ajudar?)
* [Atenção](#Atenção)

![](https://github.com/DantasB/Siga-Livre/blob/master/ReadmeFiles/Siga-Livre.gif)

### Como isso funciona?
Você primeiro precisa acessar a [Página](https://sigalivre.azurewebsites.net/) da API. Existem duas maneiras de utilizar isso.

- Primeira: Acesse a [api/Course](https://sigalivre.azurewebsites.net/api/Course) e então você irá obter todas as disciplinas abertas da faculdade neste ano
- Segunda: Acesse a [api/Course?Curso=Any course](https://sigalivre.azurewebsites.net/api/Course?Curso=Engenharia%20de%20Computação) Onde você pode escolher qualquer curso e obter toda as disciplinas que abriram para este curso, neste ano.

### Como eu uso isso?
Você só precisa fazer um GET para o site e então tratar isso em qualquer linguagem de programação que você queira. A API retorna um objeto Json que pode ser desserializado facilmente.

### Como eu posso te ajudar? 
Existe uma variavel de erro no final do objeto, então, se você aparecer algum erro, me notifique qual curso você tentou pegar a informação e então eu vou cuidar disso!

### Atenção
Essa API não tem nenhuma relação com o [Portal UFRJ](https://portalaluno.ufrj.br/Portal)
