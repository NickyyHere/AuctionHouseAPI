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

            CreateMap<CreateAuctionDTO, Auction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.Bids, opt => opt.Ignore());
            CreateMap<CreateAuctionItemDTO, AuctionItem>()
                .ForMember(dest => dest.AuctionId, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ForMember(dest => dest.Auction, opt => opt.Ignore());
            CreateMap<CreateAuctionOptionsDTO, AuctionOptions>()
                .ForMember(dest => dest.AuctionId, opt => opt.Ignore())
                .ForMember(dest => dest.Auction, opt => opt.Ignore());
        }
    }
}
