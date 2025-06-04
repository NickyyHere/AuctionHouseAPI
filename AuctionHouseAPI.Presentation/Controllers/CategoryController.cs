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
        /// <summary>
        /// Add new category
        /// </summary>
        /// <param name="createCategoryDTO">CreateCategoryDTO; Category data</param>
        /// <returns>
        /// int
        /// </returns>
        /// <response code="200">Category created</response>
        /// <response code="403">No permissions</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpPost, Authorize(Roles = "ROLE_ADMIN")]
        public async Task<ActionResult<int>> AddCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            var command = new CreateCategoryCommand(createCategoryDTO);
            var categoryId = await _mediator.Send(command);
            return Ok(categoryId);
        }
        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="id">Integer; Category id</param>
        /// <returns>
        /// CategoryDTO
        /// </returns>
        /// <response code="200">Category data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var query = new GetCategoryByIdQuery(id);
            var category = await _mediator.Send(query);
            return Ok(category);
        }
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>
        /// CategoryDTO[]
        /// </returns>
        /// <response code="200">Categories data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var query = new GetAllCategoriesQuery();
            var categories = await _mediator.Send(query);
            return Ok(categories);
        }
        /// <summary>
        /// Delete category by id
        /// </summary>
        /// <param name="id">Integer; Category id</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        /// <response code="204">Category deleted</response>
        /// <response code="403">No permissions</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpDelete("{id}"), Authorize(Roles = "ROLE_ADMIN")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var command = new DeleteCategoryCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
        /// <summary>
        /// Update category by id
        /// </summary>
        /// <param name="id">Integer; Category id</param>
        /// <param name="editedCategory">UpdateCategoryDTO; Category data</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        /// <response code="204">Category updated</response>
        /// <response code="403">No permissions</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpPut("{id}"), Authorize(Roles = "ROLE_ADMIN")]
        public async Task<ActionResult> EditCategory(int id, [FromBody] UpdateCategoryDTO editedCategory)
        {
            var command = new EditCategoryCommand(editedCategory, id);
            await _mediator.Send(command);
            return NoContent();
        }  
    }
}
