using CoreApiResponse;
using GaHipHop_Model.DTO.Request;
using GaHipHop_Model.DTO.Response;
using GaHipHop_Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tools;

namespace GaHipHop_API.Controllers.cart
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public IActionResult AddItemToCart([FromBody] AddItemToCartRequest request)
        {
            try
            {
                var cartItemDTO = _cartService.AddItem(request.Id, request.Quantity);
                return CustomResult("Item added to cart.", cartItemDTO);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (CustomException.InvalidDataException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult("An error occurred while adding the item to the cart.", HttpStatusCode.InternalServerError);
            }
        }
    }
}
