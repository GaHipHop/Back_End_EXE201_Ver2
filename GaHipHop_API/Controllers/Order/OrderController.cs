using CoreApiResponse;
using GaHipHop_Model.DTO.Request;
using GaHipHop_Repository.Entity;
using GaHipHop_Service.Interfaces;
using GaHipHop_Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using System.Net;
using Tools;

namespace GaHipHop_API.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOder([FromBody] OrderRequest orderRequest)
        {
            try
            {
                var result = await _orderService.CreateOrder(orderRequest);

                return CustomResult("Create Successful", result);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (CustomException.ForbbidenException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.Forbidden);
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }

        }


        [HttpGet("getOrder")]
        [Authorize(Roles ="Admin,Manager")]
        public IActionResult GetAllOrder(string? keyword, int pageIndex, int pageSize)
        {
            try
            {
                var order = _orderService.GetAllOrder(keyword, pageIndex, pageSize);
                return CustomResult("Data load Successful", order);
            }
            catch (CustomException.ForbbidenException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.Forbidden);
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }

        }

        [HttpGet("getOrderById/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);

                return CustomResult("Get Order successful.", order);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (CustomException.ForbbidenException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.Forbidden);
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetOrdersSummaryByMonthYear")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetOrdersSummaryByMonthYear(int month, int year)
        {
            try
            {
                var count = await _orderService.GetOrdersSummaryByMonthYear(month, year);
                return Ok(count);
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }


        [HttpPost("import")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ImportCsv(IFormFile file)
        {
            try
            {
                var import = await _orderService.ImportCsvAsync(file);
                return CustomResult("Import successful.", import);
            }catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }catch(CustomException.InvalidDataException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }catch(CustomException.InternalServerErrorException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
