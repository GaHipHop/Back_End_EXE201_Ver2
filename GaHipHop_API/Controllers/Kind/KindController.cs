﻿using AutoMapper;
using CoreApiResponse;
using GaHipHop_Model.DTO.Request;
using GaHipHop_Model.DTO.Response;
using GaHipHop_Repository.Repository;
using GaHipHop_Service.Interfaces;
using GaHipHop_Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tools;

namespace GaHipHop_API.Controllers.Kind
{
    [Route("api/[controller]")]
    [ApiController]
    public class KindController : BaseController
    {
        private readonly IKindService _kindService;

        public KindController(IKindService kindService)
        {
            _kindService = kindService;
        }

        [HttpGet("GetAllKind")]
        public async Task<IActionResult> GetAllKind([FromQuery] QueryObject queryObject)
        {
            try
            {
                var kinds = await _kindService.GetAllKind(queryObject);
                return CustomResult("Get all Data Successfully", kinds);
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

        [HttpGet("GetAllKindFalse")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllKindFalse([FromQuery] QueryObject queryObject)
        {
            try
            {
                var kind = await _kindService.GetAllKindFalse(queryObject);
                return CustomResult("Get all Data Successfully", kind);
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

        [HttpGet("GetAllKindByProductId/{id}")]
        public async Task<IActionResult> GetAllKindByProductId(long id)
        {
            try
            {
                var kinds = await _kindService.GetAllKindByProductId(id);
                return CustomResult("Product those kind:", kinds);
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

        [HttpGet("GetKindById/{id}")]
        public async Task<IActionResult> GetKindById(long id)
        {
            try
            {
                var kind = await _kindService.GetKindById(id);

                return CustomResult("Kind is found", kind);
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

        [HttpPost("CreateKind")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateKind([FromForm] KindRequest kindRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return CustomResult(ModelState, HttpStatusCode.BadRequest);
                }

                KindResponse kind = await _kindService.CreateKind(kindRequest);
                return CustomResult("Create Successful", kind, HttpStatusCode.OK);
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

        [HttpPatch("UpdateKind/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateKind(long id, [FromForm] UpdateKindRequest updateKindRequest)
        {
            try
            {
                KindResponse kind = await _kindService.UpdateKind(id, updateKindRequest);
                return CustomResult("Update Sucessfully", kind, HttpStatusCode.OK);
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
            catch (CustomException.InvalidDataException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("DeleteKind/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteKind(long id)
        {
            try
            {
                var kind = await _kindService.DeleteKind(id);
                return CustomResult("Delete Kind Successfull (Status)", kind, HttpStatusCode.OK);
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
