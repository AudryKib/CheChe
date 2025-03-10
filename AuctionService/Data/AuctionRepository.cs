using AuctionService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AuctionDBContext _context;
        public AuctionRepository(AuctionDBContext context)
        {
            _context = context;
        }
        public async Task<List<Auction>> GetAllAuctions()
        {
            return await _context.Auctions.Include(x => x.Item).ToListAsync();
        }
        public async Task<Auction> GetAuctionById(Guid id)
        {
            return await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Auction> CreateAuction(Auction auction)
        { 
            await _context.Auctions.AddAsync(auction);
            await _context.SaveChangesAsync();
            return auction;
        }
        public async Task<Auction> UpdateAuction(Auction auction)
        {
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
            return auction;
        }
        public async Task<Auction> DeleteAuction(Guid id)
        {
            var auction = await GetAuctionById(id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
                return auction;
            }
            return null;
        }
    }
    
}
