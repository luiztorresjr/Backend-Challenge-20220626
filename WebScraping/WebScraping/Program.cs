
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using WebScraping.Infra.Cron;
using WebScraping.Infra.Models;
using WebScraping.Infra.Scraping;
using WebScraping.Infra.Services;
using WebScraping.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<MongoDBSetttings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDBService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfraestrutura();
builder.Services.AddCors();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMongoDBService, MongoDBService>();
builder.Services.AddScoped<IWebScrapingService, WebScrapingService>();
builder.Services.AddScoped<IScrapingService, ScrapingService>();

builder.WebHost.ConfigureKestrel(opt=>{
    opt.ConfigureHttpsDefaults(listenOptions =>
    {
        listenOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
