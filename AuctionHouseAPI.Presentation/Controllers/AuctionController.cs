using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionHouseAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionsController : Controller
    {
        private readonly IMediator _mediator;
        public AuctionsController(IMediator mediator) 
        {
            _mediator = mediator;
        }
        // POST
        [HttpPost, Authorize]
        public async Task<ActionResult<int>> CreateAuction([FromBody] CreateAuctionDTO auction)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Problem();
            }
            var command = new CreateAuctionCommand(auction, userId);
            var auctionId = await _mediator.Send(command);
            return Ok(auctionId);
        }
        // GET
        [HttpGet]
        public async Task<ActionResult<List<AuctionDTO>>> GetAuctions()
        {
            var query = new GetAllAuctionsQuery();
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
        }
        [HttpGet("items")]
        public async Task<ActionResult<List<AuctionItemDTO>>> GetAuctionItems()
        {
            var query = new GetAllAuctionItemsQuery();
            var items = await _mediator.Send(query);
            return Ok(items);
        }
        [HttpGet("user/{uid}")]
        public async Task<ActionResult<List<AuctionDTO>>> GetUserAuctions(int uid)
        {
            var query = new GetAllAuctionsByUserQuery(uid);
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
        }
        [HttpGet("category/{cid}")]
        public async Task<ActionResult<List<AuctionDTO>>> GetCategoryAuctions(int cid)
        {
            var query = new GetAllAuctionsFromCategoryQuery(cid);
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
        }
        [HttpGet("tags")]
        public async Task<ActionResult<List<AuctionDTO>>> GetTagsAuctions([FromQuery] string[] tags)
        {
            var query = new GetAllAuctionsByTagsQuery(tags.ToList());
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDTO>> GetAuction(int id)
        {
            var query = new GetAuctionByIdQuery(id);
            var auction = await _mediator.Send(query);
            return Ok(auction);
        }
        [HttpGet("{id}/item")]
        public async Task<ActionResult<AuctionItemDTO>> GetAuctionItem(int id)
        {
            var query = new GetAuctionItemByIdQuery(id);
            var item = await _mediator.Send(query);
            return Ok(item);
        }
        [HttpGet("{id}/options")]
        public async Task<ActionResult<AuctionOptionsDTO>> GetAuctionOptions(int id)
        {
            var query = new GetAuctionOptionsByIdQuery(id);
            var options = await _mediator.Send(query);
            return Ok(options);
        }
        [HttpGet("active")]
        public async Task<ActionResult<List<AuctionDTO>>> GetActiveAuctions()
        {
            var query = new GetAllActiveAuctionsQuery();
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
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
                var command = new DeleteAuctionCommand(id, userId);
                await _mediator.Send(command);
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
                var command = new UpdateAuctionItemCommand(editedAuctionItem, id, userId);
                await _mediator.Send(command);
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
                var command = new UpdateAuctionOptionsCommand(editedAuctionOptions, id, userId);
                await _mediator.Send(command);
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
