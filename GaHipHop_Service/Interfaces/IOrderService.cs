using GaHipHop_Model.DTO.Request;
using GaHipHop_Model.DTO.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaHipHop_Service.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(OrderRequest orderRequest);
        Task<OrderResponse> GetOrderById(long id);
        List<OrderResponse> GetAllOrder(string? keyword, int pageIndex, int pageSize);
        Task<OrderSummaryResponse> GetOrdersSummaryByMonthYear(int month, int year);

        Task<bool> ImportCsvAsync(IFormFile file);
    }
}
