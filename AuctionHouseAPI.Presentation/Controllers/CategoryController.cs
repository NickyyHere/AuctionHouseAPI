using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // POST
        [HttpPost, Authorize]
        public async Task<ActionResult> AddCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            await _categoryService.CreateCategory(createCategoryDTO);
            return Created();
        }
        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategory(id);
            return Ok(category);
        }
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }
        // DELETE
        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
        // PUT
        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult> EditCategory(int id, [FromBody] UpdateCategoryDTO editedCategory)
        {
            await _categoryService.UpdateCategory(editedCategory, id);
            return NoContent();
        }  
    }
}
