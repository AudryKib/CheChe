using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Models;
using SearchService.Services;
using System.Runtime.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetRetryPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();



app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
{
    try
    {
        DbInitializer.InitDb(app).Wait();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
});

app.Run();


static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
=> HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
    .WaitAndRetryAsync(
    [
        TimeSpan.FromMilliseconds(500),
        TimeSpan.FromMilliseconds(1000),
        TimeSpan.FromMilliseconds(1500)
    ]);