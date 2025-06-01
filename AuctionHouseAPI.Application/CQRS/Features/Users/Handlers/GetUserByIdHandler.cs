using AuctionHouseAPI.Application.CQRS.Features.Users.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Users.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.id)
                ?? throw new EntityDoesNotExistException($"User with given id ({request.id}) does not exist");
            return _mapper.Map<UserDTO>(user);
        }
    }
}
