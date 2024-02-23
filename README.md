# WebScraping (Challenge - coodesh)

Este é um desafio para back-end, para testar conhecimentos na criação de API utilizando .NET/C# 
que tem como objeto ler informações de produtos, além de obter informações do site
[https://world.openfoodfacts.org/], através web scraping, utilizando o HtmlAgilityPack, que irá rodar automaticamente á meia noite todo dia obtendo 100 itens.
[ttps://lab.coodesh.com/luiztrjunior/challenge-20220626]
#Tecnologias utilizadas
- .NET 
- Quartz
- HtmlAgiliyPack
- Swagger
- MongoDB
- Docker
- Redis

# Responses 

##### Objeto retornado no get
  - Status: 200
  - Mensagem: "Fullstack Challenge 20201026"
    
##### Objeto retornado no get (products)
Deve ser informado o paginamento desejado, 
com o numero da pagina e o numero de elementos por página
  as informações devem ser:
- code
- barcode
- status
- imported
- url
- productname
- quantity
- categories
- packaging
- brands
- image_url

  ##### Objeto retornado no endpoint (product/:id)
 Retornar a apenas um elemento, caso não retorne o elemento retorna, NoContent()
  as informações devem ser:
- code
- barcode
- status
- imported
- url
- productname
- quantity
- categories
- packaging
- brands
- image_url
- 
##### Tecnologias necessárias para rodar
- .NET
- Docker Desktop
  
##### Como rodar
1. Clone o projeto
2. Na pasta raiz docker-compose up -d
3. dotnet run --project .\WebScraping\WebScraping.csproj
