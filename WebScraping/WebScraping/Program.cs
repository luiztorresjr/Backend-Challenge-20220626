
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using WebScraping.Helpers;
using WebScraping.Infra.Cron;
using WebScraping.Infra.Mappers;
using WebScraping.Infra.Models;
using WebScraping.Infra.Repository;
using WebScraping.Infra.Scraping;
using WebScraping.Infra.Services;
using WebScraping.Services;

var builder = WebApplication.CreateBuilder(args);

#region [Database]
builder.Services.Configure<MongoDBSetttings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<IMongoDBSettings>(opt => opt.GetRequiredService<IOptions<MongoDBSetttings>>().Value);
#endregion

#region [Cache]
builder.Services.AddDistributedRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetSection("Redis:ConnectionString").Value;

});
#endregion

#region [HealthCheck]
builder.Services.AddHealthChecks()
              .AddRedis(builder.Configuration.GetSection("Redis:ConnectionString").Value, tags: new string[] { "db", "data" })
             .AddMongoDb(builder.Configuration.GetSection("MongoDB:ConnectionURI").Value + "/" + builder.Configuration.GetSection("MongoDB:DatabaseName").Value,
                    name: "mongo", tags: new string[] { "db", "data" });


builder.Services.AddHealthChecksUI(opt =>
{
    opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
    opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
    opt.SetApiMaxActiveRequests(1); //api requests concurrency

    opt.AddHealthCheckEndpoint("default api", "/health"); //map health check api
}).AddInMemoryStorage();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfraestrutura();
builder.Services.AddCors();
builder.Services.AddAutoMapper(typeof(WebScraping.Helpers.ProductProfile).Assembly, typeof(WebScraping.Infra.Mappers.ProductEntityMapping).Assembly);
builder.Services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddSingleton<IProductMongoService, ProductMongoService>();
builder.Services.AddSingleton<IWebScrapingService, WebScrapingService>();
builder.Services.AddTransient<IScrapingService, ScrapingService>();

builder.Services.AddSingleton<ICacheRedisService, CacheRedisService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,

}).UseHealthChecksUI(h => h.UIPath = "/healthui");

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
