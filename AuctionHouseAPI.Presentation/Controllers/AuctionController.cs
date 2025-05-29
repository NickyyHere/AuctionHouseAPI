using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionHouseAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionsController : Controller
    {
        private readonly IAuctionService _auctionService;
        public AuctionsController(IAuctionService auctionService) 
        {
           _auctionService = auctionService;
        }
        // POST
        [HttpPost, Authorize]
        public async Task<ActionResult<int>> CreateAuction([FromBody] CreateAuctionDTO auction)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Problem();
            }
            var auctionId = await _auctionService.CreateAuction(auction, userId);
            return Ok(auctionId);
        }
        // GET
        [HttpGet]
        public async Task<ActionResult<List<AuctionDTO>>> GetAuctions()
        {
            var auctions = await _auctionService.GetAllAuctions();
            return Ok(auctions);
        }
        [HttpGet("items")]
        public async Task<ActionResult<List<AuctionItemDTO>>> GetAuctionItems()
        {
            var auctionItems = await _auctionService.GetAllAuctionItems();
            return Ok(auctionItems);
        }
        [HttpGet("user/{uid}")]
        public async Task<ActionResult<List<AuctionDTO>>> GetUserAuctions(int uid)
        {
            var auctions = await _auctionService.GetAuctionsByUser(uid);
            return Ok(auctions);
        }
        [HttpGet("category/{cid}")]
        public async Task<ActionResult<List<AuctionDTO>>> GetCategoryAuctions(int cid)
        {
            var auctions = await _auctionService.GetAuctionsByCategory(cid);
            return Ok(auctions);
        }
        [HttpGet("tags")]
        public async Task<ActionResult<List<AuctionDTO>>> GetTagsAuctions([FromQuery] string[] tags)
        {
            var auctions = await _auctionService.GetAuctionsByTags(tags);
            return Ok(auctions);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDTO>> GetAuction(int id)
        {
            var auction = await _auctionService.GetAuctionById(id);
            return Ok(auction);
        }
        [HttpGet("{id}/item")]
        public async Task<ActionResult<AuctionItemDTO>> GetAuctionItem(int id)
        {
            var item = await _auctionService.GetAuctionItem(id);
            return Ok(item);
        }
        [HttpGet("{id}/options")]
        public async Task<ActionResult<AuctionOptionsDTO>> GetAuctionOptions(int id)
        {
            var options = await _auctionService.GetAuctionOptions(id);
            return Ok(options);
        }
        // DELETE
        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult> DeleteAuction(int id)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Problem();
            }
            try
            {
                await _auctionService.DeleteAuction(id, userId);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (ActiveAuctionException e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }
        // PUT
        [HttpPut("{id}/item"), Authorize]
        public async Task<ActionResult> EditAuctionItem(int id, [FromBody] UpdateAuctionItemDTO editedAuctionItem)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Problem();
            }
            try
            {
                await _auctionService.UpdateAuctionItem(editedAuctionItem, id, userId);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (ActiveAuctionException e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }
        [HttpPut("{id}/options"), Authorize]
        public async Task<ActionResult> EditAuctionOptions(int id, [FromBody] UpdateAuctionOptionsDTO editedAuctionOptions)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Problem();
            }
            try
            {
                await _auctionService.UpdateAuctionOptions(editedAuctionOptions, id, userId);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (ActiveAuctionException e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }
    }
}
