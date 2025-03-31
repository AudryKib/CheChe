using AuctionService.Data;
using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly ILogger<BidPlacedConsumer> _logger;
        private readonly AuctionDBContext _dbContext;

        public BidPlacedConsumer(ILogger<BidPlacedConsumer> logger, AuctionDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public  async Task Consume(ConsumeContext<BidPlaced> context)
        {
            _logger.LogInformation("Bid placed for auction {AuctionId} by {Bidder} with amount {Amount}",
                context.Message.AuctionId, context.Message.Bidder, context.Message.Amount);

            var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId))
    ?? throw new MessageException(typeof(AuctionFinished), "Cannot retrieve this auction");

            if (auction.CurrentHighBid == null
                || context.Message.BidStatus.Contains("Accepted")
                && context.Message.Amount > auction.CurrentHighBid)
            {
                auction.CurrentHighBid = context.Message.Amount;
            }

            await _dbContext.SaveChangesAsync();


        }
    }
}
