using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;
using AuctionHouseAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : Controller
    {
        private readonly IAuctionService _auctionService;
        public AuctionController(IAuctionService auctionService) 
        {
           _auctionService = auctionService;
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<int>> CreateAuction([FromBody] CreateAuctionDTO auction)
        {
            var auctionId = await _auctionService.CreateAuction(auction);
            return Ok(auctionId);
        }
        [HttpDelete, Authorize]
        public async Task<ActionResult> DeleteAuction(int id)
        {
            await _auctionService.DeleteAuction(id);
            return NoContent();
        }
        [HttpPut("{id}/item"), Authorize]
        public async Task<ActionResult> EditAuctionItem(int id, [FromBody] UpdateAuctionItemDTO editedAuctionItem)
        {
            await _auctionService.UpdateAuctionItem(editedAuctionItem, id);
            return NoContent();
        }
        [HttpPut("{id}/options"), Authorize]
        public async Task<ActionResult> EditAuctionOptions(int id, [FromBody] UpdateAuctionOptionsDTO editedAuctionOptions)
        {
            await _auctionService.UpdateAuctionOptions(editedAuctionOptions, id);
            return NoContent();
        }
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<AuctionDTO>> GetAuction(int id)
        {
            var auction = await _auctionService.GetAuctionById(id);
            return Ok(auction);
        }
        [HttpGet]
        public async Task<ActionResult<List<AuctionDTO>>> GetAuctions()
        {
            var auctions = await _auctionService.GetAllAuctions();
            return Ok(auctions);
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
        [HttpGet("items")]
        public async Task<ActionResult<List<AuctionItemDTO>>> GetAuctionItems()
        {
            var auctionItems = await _auctionService.GetAllAuctionItems();
            return Ok(auctionItems);
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
        
        
    }
}
