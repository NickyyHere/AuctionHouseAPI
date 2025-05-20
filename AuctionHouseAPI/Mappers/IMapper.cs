namespace AuctionHouseAPI.Mappers
{
    public interface IMapper<DTO, CREATE_DTO, ENTITY>
    {
        public DTO ToDTO(ENTITY entity);
        public List<DTO> ToDTO(List<ENTITY> entities);
        public ENTITY ToEntity(CREATE_DTO create_dto);
    }
}
