﻿using CoreApiResponse;
using GaHipHop_Model.DTO.Request;
using GaHipHop_Model.DTO.Response;
using GaHipHop_Service.Interfaces;
using GaHipHop_Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using Tools;

namespace GaHipHop_API.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct([FromQuery] QueryObject queryObject)
        {
            try
            {
                var product = await _productService.GetAllProduct(queryObject);
                return CustomResult("Get all data Successfully", product);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetAllProductTrue")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllProductTrue([FromQuery] QueryObject queryObject)
        {
            try
            {
                var product = await _productService.GetAllProductTrue(queryObject);
                return CustomResult("Get all data Successfully", product);
            }
            catch (CustomException.ForbbidenException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.Forbidden);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
        }

        [HttpGet("GetAllProductFalse")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllProductFalse([FromQuery] QueryObject queryObject)
        {
            try
            {
                var product = await _productService.GetAllProductFalse(queryObject);
                return CustomResult("Get all data Successfully", product);
            }
            catch (CustomException.ForbbidenException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.Forbidden);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
        }

        [HttpGet("GetAllProductByCategoryId/{id}")]
        public async Task<IActionResult> GetAllProductByCategoryId(long id)
        {
            try
            {
                var products = await _productService.GetAllProductByCategoryId(id);
                return CustomResult("Category have those products:", products);
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
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            try
            {
                var product = await _productService.GetProductById(id);

                return CustomResult("Product is found", product);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductRequest productRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return CustomResult(ModelState, HttpStatusCode.BadRequest);
                }

                ProductAnyKindResponse product = await _productService.CreateProduct(productRequest);
                return CustomResult("Create Successful", product, HttpStatusCode.OK);
            }
            catch (CustomException.ForbbidenException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.Forbidden);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }

        }

        [HttpPatch("UpdateProduct/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateProduct(long id, [FromBody] ProductRequest productRequest)
        {
            try
            {
                ProductResponse product = await _productService.UpdateProduct(id, productRequest);
                return CustomResult("updated Successful", product, HttpStatusCode.OK);
            }
            catch (CustomException.ForbbidenException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.Forbidden);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (CustomException.DataExistException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            try
            {
                var deleteProduct = await _productService.DeleteProduct(id);
                return CustomResult("Delete Prodcut Successfull (Status)", deleteProduct, HttpStatusCode.OK);
            }
            catch (CustomException.ForbbidenException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.Forbidden);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }

        }
    }
}
