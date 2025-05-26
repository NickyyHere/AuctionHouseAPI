using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionHouseAPI.Presentation.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;
        public BidController(IBidService bidService)
        {
            _bidService = bidService;
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
                await _bidService.CreateBid(createBidDTO, userId);
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
        [HttpGet("by/user/{uid}")]
        public async Task<ActionResult<List<BidDTO>>> GetUserBids(int uid)
        {
            var bids = await _bidService.GetUserBids(uid);
            return Ok(bids);
        }
        [HttpGet("by/auction/{aid}")]
        public async Task<ActionResult<List<BidDTO>>> GetAuctionBids(int aid)
        {
            var bids = await _bidService.GetAuctionBids(aid);
            return Ok(bids);
        }
        [HttpGet("by/auction/{aid}/user/{uid}")]
        public async Task<ActionResult<List<BidDTO>>> GetUserAuctionBids(int aid, int uid)
        {
            var bids = await _bidService.GetUsersBidsByAuctionId(aid, uid);
            return Ok(bids);
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
                await _bidService.WithdrawFromAuction(aid, userId);
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
