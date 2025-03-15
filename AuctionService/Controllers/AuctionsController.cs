using AuctionService.Data;
using AuctionService.Models;
using AuctionService.Models.DTO;
using AutoMapper;
using Contracts;
using MassTransit;
using MassTransit.Transports;
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
        private readonly IPublishEndpoint _publishEndpoint;

        public AuctionsController(ILogger<AuctionsController> logger, IMapper mapper, IAuctionRepository auctionRepository, 
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _mapper = mapper;
            _auctionRepository = auctionRepository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date)
        { 
            var auctions = await _auctionRepository.GetAllAuctions(date);
            if (auctions == null)
            {
                return NotFound("No auctions found.");
            }
           
            return Ok(auctions);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid Id)
        {
            var auction = await _auctionRepository.GetAuctionById(Id);

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
            var newAuction = _mapper.Map<AuctionDto>(createdAuction);

            await  _publishEndpoint.Publish<AuctionCreated>(newAuction);

            var result =  await _auctionRepository.SaveAsync();

            if (!result) return BadRequest("Failed to create auction.");

            return CreatedAtAction(nameof(GetAuctionById), new { Id = createdAuction.Id }, newAuction);
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
         //   _auctionRepository.UpdateAuction(auction);

            await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));
            var result = await _auctionRepository.SaveAsync();
            if (!result) return BadRequest("Failed to update auction.");
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteAuction(Guid Id)
        {
            var auction = await _auctionRepository.GetAuctionById(Id);
            if (auction == null)
            {
                return NotFound("Auction not found.");
            }
            
            _auctionRepository.DeleteAuction(auction);

            await _publishEndpoint.Publish<AuctionDeleted>(new { Id = auction.Id.ToString() });

            var result = await _auctionRepository.SaveAsync();

            if (!result) return BadRequest("Failed to delete auction.");

            return Ok("Auction deleted successfully.");
        }
    }
}
