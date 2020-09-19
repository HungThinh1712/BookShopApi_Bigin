
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookShopApi.Controllers
{  
    public abstract class HttpController : ControllerBase
    {
        protected ActionResult successRequest(object data = null)
        {      
            if(data != null){
                string json = JsonConvert.SerializeObject(data);
            }                        
            string[] success = { "Successfully" };
            object response = new
            {
                data = data,
                error_code = 0,
                message = success[0],
            };
            return StatusCode(200, response);
        }
        protected ActionResult errorBadRequest(string message = "")
        {           
            string[] success = { "Error" };
            object response = new
            {
                data = message,
                error_code = 0,
                message = success[0],
            };
            return StatusCode(400, response);
        }
        protected ActionResult errorBadRequest(object data = null)
        {
            if (data != null)
            {
                string json = JsonConvert.SerializeObject(data);
            }
            string[] success = { "Error" };
            object response = new
            {
                data = data,
                error_code = 0,
                message = success[0],
            };
            return StatusCode(400, response);
        }
    }
}