using AuctionService.Data;
using AuctionService.Models;
using AuctionService.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly ILogger<AuctionsController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuctionRepository _auctionRepository;


        public AuctionsController(ILogger<AuctionsController> logger, IMapper mapper, IAuctionRepository auctionRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _auctionRepository = auctionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
        { 
            var auctions = await _auctionRepository.GetAllAuctions();
            if (auctions == null || !auctions.Any())
            {
                return NotFound("No auctions found.");
            }
            var auctionDtos = _mapper.Map<List<AuctionDto>>(auctions);
            return Ok(auctionDtos);
        }

        [HttpGet("{Id}")]
        public IActionResult GetAuctionById(Guid Id)
        {
            var auction = _auctionRepository.GetAuctionById(Id);
        
            if (auction == null)
            {
                return NotFound("Auction not found.");
            }
            var auctionDto = _mapper.Map<AuctionDto>(auction);
            return Ok(auctionDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuction( CreateAuctionDto auctionDto)
        {
            if (auctionDto == null)
            {
                return BadRequest("Invalid auction data.");
            }
            var auction = _mapper.Map<Auction>(auctionDto);
            auction.Seller = "Seller";
            var createdAuction = await _auctionRepository.CreateAuction(auction);
            var createdAuctionDto = _mapper.Map<AuctionDto>(createdAuction);
            return CreatedAtAction(nameof(GetAuctionById), new { Id = createdAuction.Id }, createdAuctionDto);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAuction(Guid Id, UpdateAuctionDto updateAuctionDto)
        {
         
            var auction = await _auctionRepository.GetAuctionById(Id);
            if (auction == null)
            {
                return NotFound("Auction not found.");
            }

            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

          //  var updatedAuction = _mapper.Map(updateAuctionDto, auction);
            await _auctionRepository.UpdateAuction(auction);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAuction(Guid Id)
        {
            var auction = await _auctionRepository.GetAuctionById(Id);
            if (auction == null)
            {
                return NotFound("Auction not found.");
            }
            await _auctionRepository.DeleteAuction(Id);
            return NoContent();
        }
    }
}
