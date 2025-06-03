using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Categories.Queries;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // POST
        [HttpPost, Authorize]
        public async Task<ActionResult> AddCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            var command = new CreateCategoryCommand(createCategoryDTO);
            var categoryId = await _mediator.Send(command);
            return Ok(categoryId);
        }
        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var query = new GetCategoryByIdQuery(id);
            var category = await _mediator.Send(query);
            return Ok(category);
        }
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var query = new GetAllCategoriesQuery();
            var categories = await _mediator.Send(query);
            return Ok(categories);
        }
        // DELETE
        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var command = new DeleteCategoryCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
        // PUT
        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult> EditCategory(int id, [FromBody] UpdateCategoryDTO editedCategory)
        {
            var command = new EditCategoryCommand(editedCategory, id);
            await _mediator.Send(command);
            return NoContent();
        }  
    }
}
