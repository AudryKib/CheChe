using AutoMapper;
using Contracts;
using MassTransit;
using SearchService.Models;
using MongoDB.Entities;
using System.Text.Json;

namespace SearchService.Consumers
{
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuctionUpdated> _logger;

        public AuctionUpdatedConsumer( IMapper mapper, ILogger<AuctionUpdated> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
           _logger.LogInformation($"--> consuming Auction Updated: {context.Message.Id}");
            var item = _mapper.Map<Item>(context.Message);

            var result = await DB.Update<Item>()
                .Match(a => a.ID == context.Message.Id)
                .ModifyOnly(x => new
                {
                    x.Color,
                    x.Make,
                    x.Model,
                    x.Year,
                    x.Mileage
                }, item)
                .ExecuteAsync();

            _logger.LogInformation($"Item saved to MongoDB: {JsonSerializer.Serialize(item)}");
            
        }
    }
}
