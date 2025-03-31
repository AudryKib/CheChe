using AuctionService.Models;
using AuctionService.Models.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AuctionDBContext _context;
        private readonly IMapper _mapper;

        public AuctionRepository(AuctionDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<AuctionDto>> GetAllAuctions(string? date)
        {
            var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

            if (!string.IsNullOrEmpty(date))
            {
                query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
            }

           return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<Auction> GetAuctionById(Guid id)
        {
            return await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Auction> CreateAuction(Auction auction)
        { 
            await _context.Auctions.AddAsync(auction);
            return auction;
        }
        public void UpdateAuction(Auction auction)
        {
             _context.Auctions.Update(auction);
        }

        public void DeleteAuction(Auction auction)
        {
               _context.Auctions.Remove(auction);   
        }

        public async Task<bool> SaveAsync()
        {
           return  await _context.SaveChangesAsync() > 0;
        }
    }
    
}
