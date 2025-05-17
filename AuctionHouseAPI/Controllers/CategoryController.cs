using AuctionHouseAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private List<Category> Categories { get; set; } = new();
        public CategoryController() 
        {
            Categories.Add(new Category
            {
                Id = 0,
                Name = "Name",
                Description = "Description",
            });
            Categories.Add(new Category
            {
                Id = 1,
                Name = "Name2",
                Description = "Description",
            });
            Categories.Add(new Category
            {
                Id = 2,
                Name = "Name3",
                Description = "Description",
            });
            Categories.Add(new Category
            {
                Id = 3,
                Name = "Name4",
                Description = "Description",
            });
        }
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of categories</returns>
        [HttpGet]
        public ActionResult<List<Category>> GetCategories()
        {
            return Ok(Categories);
        }
        /// <summary>
        /// Add new category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Status code</returns>
        [HttpPost]
        public ActionResult AddCategory([FromBody] Category category)
        {
            Categories.Add(category);
            return Created();
        }
        /// <summary>
        /// Edit category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="editedCategory"></param>
        /// <returns>Status code</returns>
        [HttpPut("{id}")]
        public ActionResult EditCategory(int id, [FromBody] Category editedCategory)
        {
            var category = Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = editedCategory.Name;
            category.Description = editedCategory.Description;
            return NoContent();
        }
        /// <summary>
        /// Delete category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code</returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id) 
        {
            var category = Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            Categories.Remove(category);
            return NoContent();
        }
    }
}
