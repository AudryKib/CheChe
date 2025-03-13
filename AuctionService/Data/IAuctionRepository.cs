using AuctionService.Models;
using AuctionService.Models.DTO;

namespace AuctionService.Data
{
    public interface IAuctionRepository
    {
        public Task<List<AuctionDto>> GetAllAuctions(string? date);

        public Task<Auction?> GetAuctionById(Guid id);

        public Task<Auction> CreateAuction(Auction auction);

        public Task<int> UpdateAuction(Auction auction);

        public Task<int> DeleteAuction(Guid id);
    }
}
