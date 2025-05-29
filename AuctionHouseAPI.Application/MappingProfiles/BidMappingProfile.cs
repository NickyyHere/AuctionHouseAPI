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
            CreateMap<CreateBidDTO, Bid>();
        }
    }
}
