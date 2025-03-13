using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;
using System.Text.Json;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb( WebApplication app) 
        {
            await DB.InitAsync("SearchDB", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

            await DB.Index<Item>()
                .Key(a => a.Make, KeyType.Text)
                .Key(a => a.Model, KeyType.Text)
                .Key(a => a.Color, KeyType.Text)
                .CreateAsync();

            var count = await DB.CountAsync<Item>();

            //TODO: spend some time later to understand scope and how it works
            using var scope = app.Services.CreateScope();

            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

            var auctionItems = await httpClient.GetItemsForSearchDB();

            Console.WriteLine($"Auction items count: {auctionItems.Count}");    

            if (auctionItems.Count > 0) await DB.SaveAsync(auctionItems);

        }
    }
}
