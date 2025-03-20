using MassTransit;
using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using Contracts;
using AuctionService.Data;
namespace AuctionService.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly ILogger<AuctionFinishedConsumer> _logger;
        private readonly AuctionDBContext _dbContext;

        public AuctionFinishedConsumer(ILogger<AuctionFinishedConsumer> logger, AuctionDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            _logger.LogInformation("Consuming Auction - auction finished: {AuctionId}", context.Message.AuctionId);

            var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);

            if (context.Message.ItemSold)
            {
                auction.Winner = context.Message.Winner;
                auction.SoldAmount = context.Message.Amount;
            }

            auction.Status = auction.SoldAmount > auction.ReservePrice
                ? Models.Enum.Status.Finished : Models.Enum.Status.ReserveNotMet;

            await _dbContext.SaveChangesAsync();
        }
    }

}
