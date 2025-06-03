using AuctionHouseAPI.Application.CQRS.Features.Bids.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Bids.Queries;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionHouseAPI.Presentation.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BidsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // POST
        [HttpPost, Authorize]
        public async Task<ActionResult> PlaceBid([FromBody] CreateBidDTO createBidDTO)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Problem();
            }
            try
            {
                var command = new CreateBidCommand(createBidDTO, userId);
                await _mediator.Send(command);
            }
            catch (EntityDoesNotExistException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e) when (e is MinimumOutbidException || e is InactiveAuctionException)
            {
                return BadRequest(e.Message);
            }
            return Created();
        }
        // GET
        [HttpGet("user/{uid}")]
        public async Task<ActionResult<List<BidDTO>>> GetUserBids(int uid)
        {
            var query = new GetAllUserBidsQuery(uid);
            var bids = await _mediator.Send(query);
            return Ok(bids);
        }
        [HttpGet("auction/{aid}")]
        public async Task<ActionResult<List<BidDTO>>> GetAuctionBids(int aid)
        {
            var query = new GetAllAuctionBidsQuery(aid);
            var bids = await _mediator.Send(query);
            return Ok(bids);
        }
        [HttpGet("auction/{aid}/user/{uid}")]
        public async Task<ActionResult<List<BidDTO>>> GetUserAuctionBids(int aid, int uid)
        {
            var query = new GetAllAuctionBidsByUserQuery(aid, uid);
            var bids = await _mediator.Send(query);
            return Ok(bids);
        }
        [HttpGet("auction/{aid}/highest")]
        public async Task<ActionResult<BidDTO>> GetHighestBid(int aid)
        {
            var query = new GetAuctionHighestBidQuery(aid);
            var bid = await _mediator.Send(query);
            return Ok(bid);
        }
        // DELETE
        [HttpDelete("auction/{aid}"), Authorize]
        public async Task<ActionResult> WithdrawFromAuction(int aid)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Problem();
            }
            try
            {
                var command = new DeleteAllUserBidsFromAuctionCommand(userId, aid);
                await _mediator.Send(command);
            }
            catch (EntityDoesNotExistException e)
            {
                return NotFound(e.Message);
            }
            catch (FinishedAuctionException e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }
        // PUT
    }
}
