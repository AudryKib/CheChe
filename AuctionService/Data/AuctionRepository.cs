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
        public async Task<int> UpdateAuction(Auction auction)
        {
            _context.Auctions.Update(auction);
            var result = await _context.SaveChangesAsync();
            return result;
        }
        public async Task<int> DeleteAuction(Guid id)
        {
            var auction = await GetAuctionById(id);
            var result = 0;
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
               result =  await _context.SaveChangesAsync();
                return result;
            }
            return result;
        }
    }
    
}
