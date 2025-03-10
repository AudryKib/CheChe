using AuctionService.Models;

namespace AuctionService.Data
{
    public interface IAuctionRepository
    {
        public Task<List<Auction>> GetAllAuctions();

        public Task<Auction?> GetAuctionById(Guid id);

        public Task<Auction> CreateAuction(Auction auction);

        public Task<Auction> UpdateAuction(Auction auction);

        public Task<Auction> DeleteAuction(Guid id);
    }
}
