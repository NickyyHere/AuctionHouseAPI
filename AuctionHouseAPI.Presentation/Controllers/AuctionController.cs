using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Auctions.Queries;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Shared.Exceptions;
using FluentValidation;
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
        /// <summary>
        /// Creates an auction
        /// </summary>
        /// <param name="auction">CreateAuctionDTO; Neccessary auction fields</param>
        /// <returns>
        /// int
        /// </returns>
        /// <response code="200">Auction created</response>
        /// <response code="400">Wrong input</response>
        /// <response code="401">User is not logged in</response>
        /// <response code="403">Couldn't verify user identity</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="ValidationException">Thrown when input fields are incorrect</exception>

        [HttpPost, Authorize]
        public async Task<ActionResult<int>> CreateAuction([FromBody] CreateAuctionDTO auction)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Forbid("Couldn't veify user identity");
            }
            try
            {
                var command = new CreateAuctionCommand(auction, userId);
                var auctionId = await _mediator.Send(command);
                return Ok(auctionId);
            }
            catch(ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Get all auctions
        /// </summary>
        /// <returns>
        /// AuctionDTO[]
        /// </returns>
        /// <response code="200">Auctions data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet]
        public async Task<ActionResult<List<AuctionDTO>>> GetAuctions()
        {
            var query = new GetAllAuctionsQuery();
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
        }
        /// <summary>
        /// Get all auction items
        /// </summary>
        /// <returns>
        /// AuctionItemDTO[]
        /// </returns>
        /// <response code="200">Auction items data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("items")]
        public async Task<ActionResult<List<AuctionItemDTO>>> GetAuctionItems()
        {
            var query = new GetAllAuctionItemsQuery();
            var items = await _mediator.Send(query);
            return Ok(items);
        }
        /// <summary>
        /// Get all auctions by user id
        /// </summary>
        /// <param name="uid">Integer; User id</param>
        /// <returns>
        /// AuctionDTO[]
        /// </returns>
        /// <response code="200">Auctions data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("user/{uid}")]
        public async Task<ActionResult<List<AuctionDTO>>> GetUserAuctions(int uid)
        {
            var query = new GetAllAuctionsByUserQuery(uid);
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
        }
        /// <summary>
        /// Get auctions from by category id
        /// </summary>
        /// <param name="cid">Integer; Category id</param>
        /// <returns>
        /// AuctionDTO[]
        /// </returns>
        /// <response code="200">Auctions data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("category/{cid}")]
        public async Task<ActionResult<List<AuctionDTO>>> GetCategoryAuctions(int cid)
        {
            var query = new GetAllAuctionsFromCategoryQuery(cid);
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
        }
        /// <summary>
        /// Get auctions that match at least 1 tag
        /// </summary>
        /// <param name="tags">String array; tags by name</param>
        /// <returns>
        /// AuctionDTO[]
        /// </returns>
        /// <response code="200">Auctions data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("tags")]
        public async Task<ActionResult<List<AuctionDTO>>> GetTagsAuctions([FromQuery] string[] tags)
        {
            var query = new GetAllAuctionsByTagsQuery(tags.ToList());
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
        }
        /// <summary>
        /// Get auction by id
        /// </summary>
        /// <param name="id">Integer; Auction id</param>
        /// <returns>
        /// AuctionDTO
        /// </returns>
        /// <response code="200">Auction data sent</response>
        /// <response code="404">Resource not found</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="EntityDoesNotExistException">Thrown when entity does not exist in database</exception>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDTO>> GetAuction(int id)
        {
            try
            {
                var query = new GetAuctionByIdQuery(id);
                var auction = await _mediator.Send(query);
                return Ok(auction);
            }
            catch (EntityDoesNotExistException e)
            {
                return NotFound(e.Message);
            }
        }
        /// <summary>
        /// Get auction item by auction id
        /// </summary>
        /// <param name="id">Integer; Auction id</param>
        /// <returns>
        /// AuctionItemDTO
        /// </returns>
        /// <response code="200">Auction item data sent</response>
        /// <response code="404">Resource not found</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="EntityDoesNotExistException">Thrown when entity does not exist in database</exception>
        [HttpGet("{id}/item")]
        public async Task<ActionResult<AuctionItemDTO>> GetAuctionItem(int id)
        {
            try
            {
                var query = new GetAuctionItemByIdQuery(id);
                var item = await _mediator.Send(query);
                return Ok(item);
            }
            catch(EntityDoesNotExistException e)
            {
                return NotFound(e.Message);
            }
        }
        /// <summary>
        /// Get auction options by auction id
        /// </summary>
        /// <param name="id">Integer; Auction id</param>
        /// <returns>
        /// AuctionOptionsDTO
        /// </returns>
        /// <response code="200">Auction options data sent</response>
        /// <response code="404">Resource not found</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="EntityDoesNotExistException">Thrown when entity does not exist in database</exception>
        [HttpGet("{id}/options")]
        public async Task<ActionResult<AuctionOptionsDTO>> GetAuctionOptions(int id)
        {
            var query = new GetAuctionOptionsByIdQuery(id);
            var options = await _mediator.Send(query);
            return Ok(options);
        }
        /// <summary>
        /// Get active auctions
        /// </summary>
        /// <returns>
        /// AuctionDTO[]
        /// </returns>
        /// <response code="200">Auction data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("active")]
        public async Task<ActionResult<List<AuctionDTO>>> GetActiveAuctions()
        {
            var query = new GetAllActiveAuctionsQuery();
            var auctions = await _mediator.Send(query);
            return Ok(auctions);
        }
        /// <summary>
        /// Delete auction by id
        /// </summary>
        /// <param name="id">Integer; Auction id</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        /// <response code="204">Auction deleted</response>
        /// <response code="400">Auction is ongoing</response>
        /// <response code="401">User is not the owner of the auction</response>
        /// <response code="403">Coulnd't verify user identity</response>
        /// <response code="404">Resource not found</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="UnauthorizedAccessException">Thrown when user is not the owner of the auction</exception>
        /// <exception cref="ActiveAuctionException">Thrown when auction is ongoing</exception>
        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult> DeleteAuction(int id)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Forbid("Couldn't verify user identity");
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
        /// <summary>
        /// Edit auction item by auction id
        /// </summary>
        /// <param name="id">Integer; Auction id</param>
        /// /// <param name="editedAuctionItem">UpdateAuctionItemDTO; Auction item data</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        /// <response code="204">Auction item updated</response>
        /// <response code="400">Auction is ongoing</response>
        /// <response code="401">User is not the owner of the auction</response>
        /// <response code="403">Coulnd't verify user identity</response>
        /// <response code="404">Resource not found</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="UnauthorizedAccessException">Thrown when user is not the owner of the auction</exception>
        /// <exception cref="ActiveAuctionException">Thrown when auction is ongoing</exception>
        [HttpPut("{id}/item"), Authorize]
        public async Task<ActionResult> EditAuctionItem(int id, [FromBody] UpdateAuctionItemDTO editedAuctionItem)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Forbid("Couldn't verify user identity");
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
        /// <summary>
        /// Edit auction item by auction id
        /// </summary>
        /// <param name="id">Integer; Auction id</param>
        /// /// <param name="editedAuctionOptions">UpdateAuctionOptionsDTO; Auction options data</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        /// <response code="204">Auction options updated</response>
        /// <response code="400">Auction is ongoing</response>
        /// <response code="401">User is not the owner of the auction</response>
        /// <response code="403">Coulnd't verify user identity</response>
        /// <response code="404">Resource not found</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="UnauthorizedAccessException">Thrown when user is not the owner of the auction</exception>
        /// <exception cref="ActiveAuctionException">Thrown when auction is ongoing</exception>
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
