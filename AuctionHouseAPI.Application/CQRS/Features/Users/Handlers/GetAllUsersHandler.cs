using AuctionHouseAPI.Application.CQRS.Features.Users.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Handlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetAllUsersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }
    }
}
