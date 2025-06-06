using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;

namespace AuctionHouseAPI.Application.MappingProfiles
{
    public class BidMappingProfile : Profile
    {
        public BidMappingProfile()
        {
            CreateMap<Bid, BidDTO>();
            CreateMap<CreateBidDTO, Bid>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Auction, opt => opt.Ignore())
                .ForMember(dest => dest.PlacedDateTime, opt => opt.Ignore());
        }
    }
}
