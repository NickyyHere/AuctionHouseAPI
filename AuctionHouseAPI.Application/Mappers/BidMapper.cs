using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Application.Mappers
{
    public class BidMapper : IMapper<BidDTO, CreateBidDTO, Bid>
    {
        public BidDTO ToDTO(Bid entity)
        {
            return new BidDTO(entity.UserId, entity.AuctionId, entity.Amount);
        }

        public List<BidDTO> ToDTO(List<Bid> entities)
        {
            var DTOs = new List<BidDTO>();
            foreach (var entity in entities)
            {
                DTOs.Add(ToDTO(entity));
            }
            return DTOs;
        }

        public Bid ToEntity(CreateBidDTO create_dto)
        {
            return new Bid(create_dto.AuctionId, create_dto.Amount);
        }
    }
}
