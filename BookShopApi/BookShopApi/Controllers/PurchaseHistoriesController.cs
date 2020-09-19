
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
    public class PurchaseHistoriesController : HttpController
    {
        private readonly PurchaseHistoryService _purchasehistoryService;

        public PurchaseHistoriesController(PurchaseHistoryService purchasehistoryService)
        {
            _purchasehistoryService = purchasehistoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseHistory>>> GetAll()
        {
            var purchasehistories = await _purchasehistoryService.GetAsync();
            return this.successRequest(purchasehistories);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<PurchaseHistory>> Get(
            [FromQuery(Name = "id")] string id
            )
        {
            var purchasehistory = await _purchasehistoryService.GetAsync(id);
            if (purchasehistory == null)
            {
                return this.errorBadRequest("purchasehistories not found");
            }
            return this.successRequest(purchasehistory);
        }
        [HttpPost]
        public async Task<IActionResult> Create([Required][FromBody] JObject body)
        {
            if (body["name"] == null)
                return this.errorBadRequest("purchasehistories name is required");
            PurchaseHistory purchasehistory = new PurchaseHistory();
            await _purchasehistoryService.CreateAsync(purchasehistory);
            return this.successRequest(purchasehistory);
        }
    }
}