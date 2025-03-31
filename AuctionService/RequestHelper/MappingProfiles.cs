using AuctionService.Models;
using AuctionService.Models.DTO;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Auction, AuctionDto>()
            .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Item.Make))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Item.Model))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Item.Year))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Item.Color))
            .ForMember(dest => dest.Mileage, opt => opt.MapFrom(src => src.Item.Mileage))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Item.ImageUrl));
            CreateMap<Item, AuctionDto>();
            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s));
            CreateMap<CreateAuctionDto, Item>();
            CreateMap<AuctionDto, AuctionCreated>();
            CreateMap<Auction, AuctionUpdated>().IncludeMembers(a => a.Item);
            CreateMap<Item, AuctionUpdated>();
        }
    }
}
