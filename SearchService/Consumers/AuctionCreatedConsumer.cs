using MassTransit;
using Contracts;
using AutoMapper;
using SearchService.Models;
using MongoDB.Entities;
using System.Text.Json;

namespace SearchService.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuctionCreatedConsumer> _logger;
  
        public AuctionCreatedConsumer(IMapper mapper, ILogger<AuctionCreatedConsumer> logger)
        {
            _mapper = mapper;
            _logger = logger;
            
        }
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
          Console.WriteLine("--> consuming Auction created: ", context.Message.Id);

            var item = _mapper.Map<Item>(context.Message);

            await item.SaveAsync();

            _logger.LogInformation($"Item saved to MongoDB: {JsonSerializer.Serialize(item)}");

        }
    }
    
}
