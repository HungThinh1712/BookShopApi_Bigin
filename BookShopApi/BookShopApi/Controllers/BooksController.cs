
using BookShopApi.Models;
using BookShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using System.Threading.Tasks;
using BookShopApi.Controllers;

namespace BookShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : HttpController
    {
        private readonly BookService _bookService;
        private readonly CategoryService _categoryService;
        public BooksController(BookService bookService, CategoryService categoryService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAll()
        {
            var books = await _bookService.GetAsync();
            return this.successRequest(books);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Book>> GetbyId(string id)
        {
            var book = await _bookService.GetAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            //Show category
            var category = await _categoryService.GetAsync(book.CategoryID);
            book.Category = category;

            return this.successRequest(book);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<Book>> SearchBook(string searchString)
        {
            var books = await _bookService.SearchAsync(searchString);

            if (books == null)
            {
                return NotFound();
            }
            //Show category
            foreach(var book in books)
            {
                var category = await _categoryService.GetAsync(book.CategoryID);
                book.Category = category;
            }

            return this.successRequest(books);
        }

        [HttpPost]
       
        public async Task<IActionResult> Create(Book createbook)
        {
            //Check book existed or not
             var book = await _bookService.GetAsync(createbook.Code);
            //Book is not existed
            if (book == null)
            {
                await _bookService.CreateAsync(createbook);
                return this.successRequest(createbook);
            }
            //Thông báo sách đã tồn tại
            else
            {
                return this.errorBadRequest("Book is already existed");
            }
          
        }
        [HttpPut]
       
        public async Task<IActionResult> Update(string id, Book updatedBook)
        {        

            await _bookService.UpdateAsync(id,updatedBook);
            return this.successRequest("Update Sucessfully");
        }
        [HttpDelete] 
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _bookService.GetAsync(id);
            if (book == null)
            {
                return this.errorBadRequest("Not found");
            }
            book.isDeleted = true;
            await _bookService.UpdateAsync(id, book);
            return this.successRequest("Delete sucessfully");
        }
    }
}