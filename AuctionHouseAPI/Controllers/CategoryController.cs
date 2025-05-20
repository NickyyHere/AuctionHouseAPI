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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost, Authorize]
        public async Task<ActionResult> AddCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            await _categoryService.CreateCategory(createCategoryDTO);
            return Created();
        }
        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult> EditCategory(int id, [FromBody] UpdateCategoryDTO editedCategory)
        {
            await _categoryService.UpdateCategory(editedCategory, id);
            return NoContent();
        }
        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategory(id);
            return Ok(category);
        }
        [HttpGet]
        public ActionResult<List<CategoryDTO>> GetCategories()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }    
    }
}
