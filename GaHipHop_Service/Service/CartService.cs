using AutoMapper;
using GaHipHop_Model.DTO.Response;
using GaHipHop_Repository.Entity;
using GaHipHop_Repository.Repository;
using GaHipHop_Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using static System.Net.Mime.MediaTypeNames;

namespace GaHipHop_Service.Service
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private List<CartItem> GetCartItemsFromSession()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartItemsJson = session.GetString("CartItems");
            return cartItemsJson == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartItemsJson);
        }

        private void SaveCartItemsToSession(List<CartItem> cartItems)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartItemsJson = JsonConvert.SerializeObject(cartItems);
            session.SetString("CartItems", cartItemsJson);
        }

        public CartItem AddItem(long id, int quantity)
        {
            var productKind = _unitOfWork.KindRepository.GetByID(id);
            if (productKind == null)
            {
                throw new CustomException.DataNotFoundException("Product not found.");
            }

            if (quantity > productKind.Quantity)
            {
                throw new CustomException.InvalidDataException("Requested quantity is greater than the available stock.");
            }
            var product = _unitOfWork.ProductRepository.GetByID(productKind.ProductId);
            if (product.DiscountId != null)
            {
                var discount = _unitOfWork.DiscountRepository.GetByID(product.DiscountId);

                if (discount != null && discount.ExpiredDate >= DateTime.Now && discount.Status)
                {
                    var discountedPrice = product.ProductPrice * (1 - discount.Percent / 100);
                    product.ProductPrice = discountedPrice;
                }
            }

            var cartItems = GetCartItemsFromSession();
            var existingItem = cartItems.FirstOrDefault(item => item.Id == id);
            if (existingItem != null)
            {
                if (existingItem.Quantity + quantity > productKind.Quantity)
                {
                    throw new CustomException.InvalidDataException("Requested quantity exceeds the available stock.");
                }
                existingItem.Quantity += quantity;
            }
            else
            {
                cartItems.Add(new CartItem { Id = id, Quantity = quantity, ProductPrice = product.ProductPrice, ProductName = product.ProductName, ProductImage = productKind.Image, Color = productKind.ColorName });
            }
            SaveCartItemsToSession(cartItems);
            return _mapper.Map<CartItem>(existingItem ?? cartItems.Last());
        }

    }
}
