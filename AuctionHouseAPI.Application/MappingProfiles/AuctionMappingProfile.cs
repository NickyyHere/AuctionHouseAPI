using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;

namespace AuctionHouseAPI.Application.MappingProfiles
{
    public class AuctionMappingProfile : Profile
    {
        public AuctionMappingProfile()
        {
            CreateMap<Auction, AuctionDTO>()
                .ForMember(dest => dest.OwnerFirstName, opt => opt.MapFrom(src => src.Owner!.FirstName))
                .ForMember(dest => dest.OwnerLastName, opt => opt.MapFrom(src => src.Owner!.LastName))
                .ForPath(dest => dest.Item.Tags, opt => opt.MapFrom(src => src.Item!.Tags.Select(t => t.Tag!.Name).ToList()));
            CreateMap<AuctionItem, AuctionItemDTO>();
            CreateMap<AuctionOptions, AuctionOptionsDTO>();

            CreateMap<CreateAuctionDTO, Auction>();
            CreateMap<CreateAuctionItemDTO, AuctionItem>();
            CreateMap<CreateAuctionOptionsDTO, AuctionOptions>();
        }
    }
}
