using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    {
        private readonly ILogger<AuctionDeleted> _logger;

        public AuctionDeletedConsumer(ILogger<AuctionDeleted> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            _logger.LogInformation($"--> Auction Deleted: {context.Message.Id}");

            // Delete the item from MongoDB
            var itemId = context.Message.Id;
            var item = DB.Find<Item>().OneAsync(itemId).Result;
            if (item != null)
            {
                 await DB.DeleteAsync<Item>(item.ID);
                _logger.LogInformation($"Item with ID {itemId} deleted from MongoDB.");
            }
            else
            {
                _logger.LogWarning($"Item with ID {itemId} not found in MongoDB.");
            }
           
        }
    }

}
