using AutoMapper;
using GaHipHop_Model.DTO.Request;
using GaHipHop_Model.DTO.Response;
using GaHipHop_Repository.Entity;
using GaHipHop_Repository.Repository;
using GaHipHop_Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tools;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace GaHipHop_Service.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ICartService cartService, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
        /*public async Task<OrderResponse> CreateOrder(OrderRequest orderRequest)
        {

            Random rand = new Random();
            int randomNumber = rand.Next(10000, 99999);

            var cartItems = _cartService.GetCartItems();

            var userInfo = new UserInfo
            {
                UserName = orderRequest.UserName,
                Email = orderRequest.Email,
                Phone = orderRequest.Phone,
                Address = orderRequest.Address,
                Province = orderRequest.Province,
                Wards = orderRequest.Wards
            };
            _unitOfWork.UserInfoRepository.Insert(userInfo);
            _unitOfWork.Save();

            var order = _mapper.Map<Order>(orderRequest);

            var checkOrderCode = _unitOfWork.OrderRepository.Get(filter: cod => cod.OrderCode == order.OrderCode);

            order.UserId = userInfo.Id;
            order.OrderRequirement = orderRequest.OrderRequirement;
            order.PaymentMethod = orderRequest.PaymentMethod;
*//*            order.AdminId = 1;*//*

            while (_unitOfWork.OrderRepository.Get(filter: cod => cod.OrderCode == order.OrderCode).Any())
            {
                randomNumber = new Random().Next(10000, 99999);
                order.OrderCode = "ORD" + randomNumber.ToString("D5");
            }

            order.OrderCode = "ORD" + randomNumber.ToString("D5");


            order.CreateDate = DateTime.Now;
            order.TotalPrice = cartItems.TotalPrice;
            order.Status = "Pending";

            _unitOfWork.OrderRepository.Insert(order);
            _unitOfWork.Save();

            foreach(var cartItem in cartItems.Items)
            {
                var orderDetail = _mapper.Map<OrderDetails>(cartItem);

                orderDetail.OrderId = order.Id;

                orderDetail.OrderPrice = cartItem.ProductPrice;

                orderDetail.OrderQuantity = cartItem.Quantity;

                orderDetail.KindId = cartItem.Id;

                _unitOfWork.OrderDetailsRepository.Insert(orderDetail);
            }

            _cartService.ClearCart();

            return await Task.FromResult(_mapper.Map<OrderResponse>(order));
        }*/

        public async Task<OrderResponse> CreateOrder(OrderRequest orderRequest)
        {
            Random rand = new Random();
            int randomNumber = rand.Next(10000, 99999);

            var userInfo = new UserInfo
            {
                UserName = orderRequest.UserName,
                Email = orderRequest.Email,
                Phone = orderRequest.Phone,
                Address = orderRequest.Address,
            };
            _unitOfWork.UserInfoRepository.Insert(userInfo);
            _unitOfWork.Save();

            var order = _mapper.Map<Order>(orderRequest);

            var checkOrderCode = _unitOfWork.OrderRepository.Get(filter: cod => cod.OrderCode == order.OrderCode);

            order.UserId = userInfo.Id;

            while (_unitOfWork.OrderRepository.Get(filter: cod => cod.OrderCode == order.OrderCode).Any())
            {
                randomNumber = new Random().Next(10000, 99999);
                order.OrderCode = "ORD" + randomNumber.ToString("D5");
            }

            order.OrderCode = "ORD" + randomNumber.ToString("D5");
            order.CreateDate = DateTime.Now;
            order.TotalPrice = orderRequest.CartItems.Sum(item => item.ProductPrice * item.Quantity);

            _unitOfWork.OrderRepository.Insert(order);
            _unitOfWork.Save();

            foreach (var cartItem in orderRequest.CartItems)
            {
                var orderDetail = new OrderDetails
                {
                    OrderId = order.Id,
                    OrderPrice = cartItem.ProductPrice,
                    OrderQuantity = cartItem.Quantity,
                    KindId = cartItem.Id
                };

                _unitOfWork.OrderDetailsRepository.Insert(orderDetail);
            }

            _unitOfWork.Save();

            return await Task.FromResult(_mapper.Map<OrderResponse>(order));
        }

        public List<OrderResponse> GetAllOrder(string? keyword, int pageIndex, int pageSize)
        {
            try
            {
                //var checkadmin = AdminPermissionHelper.CheckAdminPermissionAndRetrieveInfo(_httpContextAccessor, _unitOfWork);

                Expression<Func<Order, bool>> filter = s => (string.IsNullOrEmpty(keyword) ||
                                                            s.OrderCode.Contains(keyword) || 
                                                            s.UserInfo.Email.Contains(keyword) ||
                                                            s.UserInfo.UserName.Contains(keyword));

                var listOrder = _unitOfWork.OrderRepository.Get(
                    filter: filter,
                    includeProperties: "UserInfo,OrderDetails,OrderDetails.Kind,OrderDetails.Kind.Product",
                    orderBy: q => q.OrderByDescending(s => s.CreateDate),
                    pageIndex: pageIndex,
                    pageSize: pageSize
                );
                var OrderResponses = _mapper.Map<List<OrderResponse>>(listOrder);
                return OrderResponses;
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                throw new CustomException.InternalServerErrorException(/*"An error occurred during data processing."*/ ex.Message);
            }
        }

        public async Task<OrderResponse> GetOrderById(long id)
        {
            try
            {

                var order = _unitOfWork.OrderRepository.Get(
                    filter: o => o.Id == id, includeProperties: "UserInfo,OrderDetails"
                ).FirstOrDefault();

                if (order == null)
                {
                    throw new CustomException.DataNotFoundException("Order not found.");
                }

                var orderResponse = _mapper.Map<OrderResponse>(order);
                return orderResponse;
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                throw new CustomException.InternalServerErrorException("An error occurred during data processing.", ex);
            }
        }

        public async Task<OrderSummaryResponse> GetOrdersSummaryByMonthYear(int month, int year)
        {
            try
            {
                var orders = _unitOfWork.OrderRepository.Get(
                    filter: o => o.CreateDate.Month == month && o.CreateDate.Year == year,
                    includeProperties: "UserInfo,OrderDetails,OrderDetails.Kind,OrderDetails.Kind.Product"
                ).ToList();

                var previousMonthOrders = _unitOfWork.OrderRepository.Get(
                    filter: o => o.CreateDate.Month == month - 1 && o.CreateDate.Year == year,
                    includeProperties: "UserInfo,OrderDetails,OrderDetails.Kind,OrderDetails.Kind.Product"
                ).ToList();

                // Tính Count và TotalAmount
                int count = orders.Count;
                double totalAmount = orders.Sum(o => o.TotalPrice);
                int quantitySold = orders.Sum(o => o.OrderDetails.Sum(qs => qs.OrderQuantity));


                int countPreviousMonth = previousMonthOrders.Count;
                double totalAmountPreviousMonth = previousMonthOrders.Sum(o => o.TotalPrice);

                var mostSoldProductGroup = orders
                    .SelectMany(o => o.OrderDetails)
                    .GroupBy(od => od.Kind.Product)
                    .OrderByDescending(g => g.Sum(od => od.OrderQuantity))
                    .FirstOrDefault();

                var mostSoldProduct = mostSoldProductGroup?.Key?.ProductName ?? "Not product.";
                var mostSoldProductQuantity = mostSoldProductGroup?.Sum(od => od.OrderQuantity) ?? 0;
                var orderResponses = _mapper.Map<List<OrderResponse>>(orders);

                var orderSummaryResponse = new OrderSummaryResponse
                {
                    Orders = orderResponses,
                    Count = count,
                    TotalAmount = totalAmount,
                    quantitySold = quantitySold,
                    MostSoldProduct = mostSoldProduct,
                    MostSoldProductQuantity = mostSoldProductQuantity,
                    CountPreviousMonth = countPreviousMonth,
                    TotalAmountPreviousMonth = totalAmountPreviousMonth
                };

                return await Task.FromResult(orderSummaryResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting orders summary: " + ex.Message);
            }
        }

        public async Task<bool> ImportCsvAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new CustomException.InvalidDataException("File empty.");

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                string line;
                bool isFirstLine = true;

                while ((line = await stream.ReadLineAsync()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue; // skip header line
                    }

                    var values = line.Split(',');

                    // Tìm user
                    var userInfo = _unitOfWork.UserInfoRepository.Get(u => u.UserName == values[0] && u.Email == values[1]).FirstOrDefault();
                    if (userInfo == null)
                    {
                        userInfo = new UserInfo
                        {
                            UserName = values[0],
                            Email = values[1],
                            Phone = values[2],
                            Address = values[3]
                        };
                         _unitOfWork.UserInfoRepository.Insert(userInfo);
                         _unitOfWork.Save();
                    }

                    // Tìm KindId từ ProductName và ColorName
                    var productName = values[7];
                    var colorName = values[8];
                    var kind = _unitOfWork.KindRepository.Get(k => k.Product.ProductName == productName && k.ColorName == colorName).FirstOrDefault();

                    if (kind == null)
                    {
                        throw new CustomException.DataNotFoundException("not found.");
                    }

                    var order = new Order
                    {
                        UserId = userInfo.Id,
                        OrderCode = values[4],
                        CreateDate = DateTime.Parse(values[5]),
                        TotalPrice = (double)decimal.Parse(values[6]),
                        OrderDetails = new List<OrderDetails>()
                    };

                    var orderDetail = new OrderDetails
                    {
                        KindId = kind.Id,
                        OrderQuantity = int.Parse(values[9]),
                        OrderPrice = (double)decimal.Parse(values[10])
                    };

                    order.OrderDetails.Add(orderDetail);

                    // Add order to the context
                    _unitOfWork.OrderRepository.Insert(order);
                }
            }

            _unitOfWork.Save();

            return true;
        }
    }
}
