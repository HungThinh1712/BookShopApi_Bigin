using BookShopApi.Models;
using BookShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BookShopApi.Controllers;

namespace BookShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: HttpController
    {
        private readonly UserService _userService;
        private readonly ShoppingCartService _shoppingcartService;

        public UsersController(UserService userService, ShoppingCartService shoppingcartService)
        {
            _userService = userService;
            _shoppingcartService = shoppingcartService;
           
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAsync();
            return this.successRequest(users);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return this.successRequest(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User creatingUser)
        {


                string username = creatingUser.UserName;
                var checkedUser = await _userService.GetUserAsync(username);
                //Check username existed or not
                if (checkedUser == null)
                {
                    
                    var createdUser = await _userService.CreateAsync(creatingUser);

                    //Create shopping cart each time create user
                    var shoppingcart = new ShoppingCart()
                    {
                        BookIds = new List<string>(),
                        UserId = createdUser.Id
                    };
                    await _shoppingcartService.CreateAsync(shoppingcart);
                    return this.successRequest(createdUser);
                }
                else
                {
                    return this.errorBadRequest("User Name is existed");
                }

               
            
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePassword(string username, string password)
        {

            var user = await _userService.GetUserAsync(username);
            user.PassWord = password;

            await _userService.UpdateAsync(user.Id,user);
            return this.successRequest(user);

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            
            var user = await _userService.GetAsync(id);
            if (user == null)
            {
                return this.errorBadRequest("Not found");
            }
            user.isDeleted = true;
            await _userService.UpdateAsync(id, user);
            return this.successRequest("Delete sucessfully");
        }

    }
}
