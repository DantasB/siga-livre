# Siga-Livre
This is a simple API that gets every opened discipline in the Federal University of Rio de Janeiro. 
Click here to read in [Portuguese](README.pt.md)

![](https://github.com/DantasB/Siga-Livre/blob/master/ReadmeFiles/Siga-Livre.gif)

## How does it work?
You first need to access the [website](https://sigalivre.azurewebsites.net/) of the api. There's two ways to use it.

- One: join the [api/Course](https://sigalivre.azurewebsites.net/api/Course) and then you can get every opened disciplines of the University, in this year
- Two: join the [api/Course?Curso=Any course](https://sigalivre.azurewebsites.net/api/Course?Curso=Engenharia de Computação) where you can choose any course and check every opened disciplines of this course, in this year.

## How can i use it?
You just need to make a GET to the website and them treat it in any Programming Language that you want. The api returns you a json object that can be easily decompressed.

## How can i help you ? 
There's an error variable in the end of the object, so, if you get any error, please notify me which course have you tried to get the information and them i'll handle it!

## Attention
This API doesn't have any relation with the [Portal UFRJ](https://portalaluno.ufrj.br/Portal)
