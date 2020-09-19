using BookShopApi.Models;
using BookShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookShopApi.Controllers;

namespace BookShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController: HttpController
    {

        private readonly ShoppingCartService _shoppingcartService;
        private readonly BookService _bookService;

        public ShoppingCartsController(ShoppingCartService shoppingcartService,BookService bookService)
        {
            _shoppingcartService = shoppingcartService;
            _bookService = bookService;
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> AddtoCart(string userid, string bookid)
        {         

            //Get Shopping cart by user id
            var shoppingcart = await _shoppingcartService.GetAsync(userid);
            //Init list book id
            if (shoppingcart.BookIds == null || shoppingcart.BookIds.Count==0 )
            {
                shoppingcart.BookIds = new List<string>();
                shoppingcart.BookIds.Add(bookid);
            }
            else
            {
                shoppingcart.BookIds.Add(bookid);

            }
            //Update list id
            await _shoppingcartService.UpdateAsync(shoppingcart.Id, shoppingcart);

            SaveBooksinCart(userid); 
            return this.successRequest("Add sucessfully");
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> RemovefromCart(string userid, string bookid)
        {
            
            //Get Shopping cart by user id
            var shoppingcart = await _shoppingcartService.GetAsync(userid);

           //Remove from cart use for beacause list have many book id
           for(int i = 0; i < shoppingcart.BookIds.Count; i++)
            {
                if (shoppingcart.BookIds[i] == bookid)
                {
                    shoppingcart.BookIds.Remove(shoppingcart.BookIds[i]);
                    i--;
                } 
            }
            foreach (var item in shoppingcart.Books)
            {
                if (item.Id == bookid)
                {
                    shoppingcart.Books.Remove(item);
                    break;
                }
            }

            await _shoppingcartService.UpdateAsync(shoppingcart.Id, shoppingcart);

            return this.successRequest("Remove sucessfully");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ShowCart(string userid)
        {
            
            //Get Shopping cart by user id
            var shoppingcart = await _shoppingcartService.GetAsync(userid);
            return this.successRequest(shoppingcart);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetTotalMoney(string userid)
        {
            decimal total = 0;
            var shoppingcart = await _shoppingcartService.GetAsync(userid);
            foreach(var item in shoppingcart.Books)
            {
                total += item.Price * item.Amount;
            }

            return this.successRequest(total);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> PayShoppingCart(string userid, List<string> bookids)
        {
            
            //Get Shopping cart by user id
            var shoppingcart = await _shoppingcartService.GetAsync(userid);

            //Check book is out of or not
            foreach (var bookid in bookids)
            {
                
                var bookInDb = await _bookService.GetAsync(bookid);

                foreach(var bookInCart in shoppingcart.Books)
                {
                    //Book in Cart are more than book in DB
                    if (bookid == bookInCart.Id && bookInCart.Amount > bookInDb.Amount)
                    {
                        bookInCart.Amount = bookInDb.Amount;
                        bookInDb.Amount = 0;

                        //Remove book in cart
                        shoppingcart.Books.Remove(bookInCart);
                        break;
                    }
                    if(bookid == bookInCart.Id && bookInCart.Amount <= bookInDb.Amount)
                    {
                        bookInDb.Amount = bookInDb.Amount - bookInCart.Amount;
                        shoppingcart.Books.Remove(bookInCart);
                        break;
                    }
                }
                await _bookService.UpdateAsync(bookInDb.Id, bookInDb);

                //Remove id from lst id
                for (int i = 0; i < shoppingcart.BookIds.Count; i++)
                {
                    if (shoppingcart.BookIds[i] == bookid)
                    {
                        shoppingcart.BookIds.Remove(shoppingcart.BookIds[i]);
                        i--;
                    }
                }

            }
            //Remove item from cart
            await _shoppingcartService.UpdateAsync(shoppingcart.Id, shoppingcart);
            return this.successRequest("Paid sucessfully");
        }

        private int CountIdinList(string id, List<string> lst)
        {
            int count = 0;
            foreach(string i in lst)
            {
                if (i == id)
                    count++;
            }
            return count;
        }

        private bool CheckIdinList(string id, List<string> lst)
        {
            foreach(var i in lst)
            {
                if (i == id)
                    return true;
            }
            return false;
        }
        private async void SaveBooksinCart(string userid)
        {
            var shoppingcart = await _shoppingcartService.GetAsync(userid);


            if (shoppingcart.BookIds.Count > 0)
            {
                var tempList = new List<Book>();

                //List check have done get book or not yet
                var checkidList = new List<string>();
                foreach (var bookid in shoppingcart.BookIds)
                {
                    if (CheckIdinList(bookid, checkidList) == false)
                    {
                        checkidList.Add(bookid);

                        //Get book from database 
                        var bookInDB = await _bookService.GetAsync(bookid);

                        //Book in cart
                        var bookInCart = new Book()
                        {
                            Id = bookInDB.Id,
                            Name = bookInDB.Name,
                            Price = bookInDB.Price,
                            CategoryID = bookInDB.CategoryID,
                            Author = bookInDB.Author,
                            Amount = CountIdinList(bookid, shoppingcart.BookIds),
                            Code = bookInDB.Code
                        };
                        tempList.Add(bookInCart);

                    }
                }
                shoppingcart.Books = tempList;
                //Save cart
                await _shoppingcartService.UpdateAsync(shoppingcart.Id, shoppingcart);
            }
        }
    }
}
