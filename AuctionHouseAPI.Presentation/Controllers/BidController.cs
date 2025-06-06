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
        /// <summary>
        /// Creates new bid on auction
        /// </summary>
        /// <param name="createBidDTO">CreateBidDTO; Bid data</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        /// <response code="400">Bid is too low or auction is inactive</response>
        /// <response code="401">User is not logged in</response>
        /// <response code="403">Couldn't verify user identity</response>
        /// <response code="404">Auction does not exist</response>
        /// <response code="409">Bid on own auction</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="EntityDoesNotExistException">Thrown when entity does not exist in database</exception>
        /// <exception cref="MinimumOutbidException">Thrown when new bid is too low</exception>
        /// <exception cref="InactiveAuctionException">Thrown when auction is inactive</exception>
        [HttpPost, Authorize]
        public async Task<ActionResult> PlaceBid([FromBody] CreateBidDTO createBidDTO)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Forbid("Couldn't verify user identity");
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
            catch (BidOnOwnedAuctionException e)
            {
                return Conflict(e.Message);
            }
            return Created();
        }
        /// <summary>
        /// Get bids by user id
        /// </summary>
        /// <param name="uid">Integer; User id</param>
        /// <returns>
        /// BidDTO[]
        /// </returns>
        /// <response code="200">Bids data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("user/{uid}")]
        public async Task<ActionResult<List<BidDTO>>> GetUserBids(int uid)
        {
            var query = new GetAllUserBidsQuery(uid);
            var bids = await _mediator.Send(query);
            return Ok(bids);
        }
        /// <summary>
        /// Get bids by auction id
        /// </summary>
        /// <param name="aid">Integer; Auction id</param>
        /// <returns>
        /// BidDTO[]
        /// </returns>
        /// <response code="200">Bids data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("auction/{aid}")]
        public async Task<ActionResult<List<BidDTO>>> GetAuctionBids(int aid)
        {
            var query = new GetAllAuctionBidsQuery(aid);
            var bids = await _mediator.Send(query);
            return Ok(bids);
        }
        /// <summary>
        /// Get bids by user id and user id
        /// </summary>
        /// <param name="uid">Integer; User id</param>
        /// <param name="aid">Integer; Auction id</param>
        /// <returns>
        /// BidDTO[]
        /// </returns>
        /// <response code="200">Bids data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("auction/{aid}/user/{uid}")]
        public async Task<ActionResult<List<BidDTO>>> GetUserAuctionBids(int aid, int uid)
        {
            var query = new GetAllAuctionBidsByUserQuery(aid, uid);
            var bids = await _mediator.Send(query);
            return Ok(bids);
        }
        /// <summary>
        /// Get highest bid of auction
        /// </summary>
        /// <param name="aid">Integer; Auction id</param>
        /// <returns>
        /// BidDTO
        /// </returns>
        /// <response code="200">Bid data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("auction/{aid}/highest")]
        public async Task<ActionResult<BidDTO>> GetHighestBid(int aid)
        {
            var query = new GetAuctionHighestBidQuery(aid);
            var bid = await _mediator.Send(query);
            return Ok(bid);
        }
        /// <summary>
        /// Delete all user bids from auction
        /// </summary>
        /// <param name="aid">Integer; Auction id</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        /// <response code="204">Bids deleted</response>
        /// <response code="400">Auction is over</response>
        /// <response code="401">User is not logged in</response>
        /// <response code="403">Couldn't verify user identity</response>
        /// <response code="404">Auction not found</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="EntityDoesNotExistException">Thrown when entity does not exist in database</exception>
        /// <exception cref="FinishedAuctionException">Thrown when auction is already finished</exception>
        [HttpDelete("auction/{aid}"), Authorize]
        public async Task<ActionResult> WithdrawFromAuction(int aid)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Forbid("Couldn't verify user identity");
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
    }
}
