using AuctionHouseAPI.Application.DTOs.Read;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseAPI.Application.CQRS.Features.Auth.Queries
{
    public record GetAuthTokenQuery(LoginDTO LoginDTO) : IRequest<string>;
}
