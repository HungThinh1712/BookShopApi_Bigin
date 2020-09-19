using BookShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookShopApi.Controllers;

namespace BookShopApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : HttpController
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }
        protected string GetToken(string id)
        {
            //security key
            string securityKey = "NHTIMH281019990965451243990182DKDT7979";

            //symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            //Get user_id from params 
            claims.Add(new Claim("sub", id.ToString()));

            //create token
            var token = new JwtSecurityToken(
                expires: DateTime.MaxValue,
                signingCredentials: signingCredentials,
                claims: claims
            );

            // return token
            return (new JwtSecurityTokenHandler().WriteToken(token));
        }


        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            var user = await _userService.GetUserAsync(username, password);
            if (user != null)
            {
                return this.successRequest(GetToken(user.Id));
            }
            else
                return this.errorBadRequest("User Name or Password is incorrect");   

        }
        
    }
}
