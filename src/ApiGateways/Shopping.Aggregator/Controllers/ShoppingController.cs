using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;
        private readonly IOrderService _orderService;
        private readonly ILogger<ShoppingController> _logger;

        public ShoppingController(IBasketService basketService,
                                  ICatalogService catalogService,
                                  IOrderService orderService,
                                  ILogger<ShoppingController> logger)
        {
            _basketService = basketService;
            _catalogService = catalogService;
            _orderService = orderService;
            _logger = logger;
        }


        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            // get basket with userName
            var basket = await _basketService.GetBasket(userName);
            // iterate basket items and consume products with basket item productId member
            // map product related members into basketItem dto with extended columns
            foreach (var item in basket.Items)
            {
                var product = await _catalogService.GetCatalog(item.ProductId);

                // set additional product fields
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            // consume Ordering microservices in order to reterive Order list
            var orders = await _orderService.GetOrdersByUserName(userName);

            //return root ShoppingModel dto class which including all responses
            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };

            return Ok(shoppingModel);
        }
    }
}
