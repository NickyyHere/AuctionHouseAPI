using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;
using AuctionHouseAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Controllers
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
        [HttpPost, Authorize]
        public async Task<ActionResult> PlaceBid([FromBody] CreateBidDTO createBidDTO)
        {
            await _bidService.CreateBid(createBidDTO);
            return Created();
        }
        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult> EditBid(int id, [FromBody] UpdateBidDTO updateBidDTO)
        {
            await _bidService.UpdateBid(updateBidDTO, id);
            return NoContent();
        }
        [HttpDelete("auction/{aid}/user/{uid}"), Authorize]
        public async Task<ActionResult> WithdrawFromAuction(int aid)
        {
            await _bidService.WithdrawFromAuction(aid);
            return NoContent();
        }
        [HttpGet("user/{uid}")]
        public async Task<ActionResult<List<BidDTO>>> GetUserBids(int uid)
        {
            var bids = await _bidService.GetUserBids(uid);
            return Ok(bids);
        }
        [HttpGet("auction/{aid}")]
        public async Task<ActionResult<List<BidDTO>>> GetAuctionBids(int aid)
        {
            var bids = await _bidService.GetAuctionBids(aid);
            return Ok(bids);
        }
        [HttpGet("auction/{aid}/user/{uid}")]
        public async Task<ActionResult<List<BidDTO>>> GetUserAuctionBids(int aid, int uid)
        {
            var bids = await _bidService.GetUsersBidsByAuctionId(aid, uid);
            return Ok(bids);
        } 
    }
}
