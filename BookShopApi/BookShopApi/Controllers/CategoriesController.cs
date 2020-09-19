
using BookShopApi.Models;
using BookShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : HttpController
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
     
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            var categories = await _categoryService.GetAsync();
            return this.successRequest(categories);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Category>> GetbyId(string id)
        {
            var category = await _categoryService.GetAsync(id);
            if (category == null)
            {
                return this.errorBadRequest("Category not found");
            }
            return this.successRequest(category);
        }
        [HttpPost]
   
        public async Task<IActionResult> Create(Category category)
        {
            await _categoryService.CreateAsync(category);
            return this.successRequest(category);
        }

        [HttpPut]    
        public async Task<IActionResult> Update(string id, Category updatedCategory)
        {
            var catergoy = await _categoryService.GetAsync(id);
            if (catergoy == null)
            {
                return this.errorBadRequest("Category not found");
            }
            await _categoryService.UpdateAsync(id, updatedCategory);
            return this.successRequest("Updated Successfully");
        }

        [HttpDelete] 
        public async Task<IActionResult> Delete(string id)
        {
            
            var category = await _categoryService.GetAsync(id);
            if (category == null)
            {
                return this.errorBadRequest("Not found");
            }
            category.isDeleted = true;
            await _categoryService.UpdateAsync(id,category);
            return this.successRequest("Delete sucessfully");
        }
    }
}