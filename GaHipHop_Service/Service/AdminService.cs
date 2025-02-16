﻿using AutoMapper;
using GaHipHop_Model.DTO.Request;
using GaHipHop_Model.DTO.Response;
using GaHipHop_Repository.Entity;
using GaHipHop_Repository.Repository;
using GaHipHop_Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace GaHipHop_Service.Service
{
    public class AdminService : IAdminService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminService(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<AdminResponse> GetAllAdminByStatusTrue()
        {
            /*var accountId = Authentication.GetRoleFromHttpContext(_httpContextAccessor.HttpContext);
            if (string.IsNullOrEmpty(accountId))
            {
                throw new CustomException.ForbbidenException("User ID claim invalid.");
            }*/

            var listAdmin = _unitOfWork.AdminRepository.Get(
                filter: s => s.Status == true && s.RoleId == 1,
                includeProperties: "Role"
            ).ToList();
            var adminResponses = _mapper.Map<IEnumerable<AdminResponse>>(listAdmin);
            return adminResponses;
        }

        public IEnumerable<AdminResponse> GetAllAdminByStatusFalse()
        {
            try
            {

                var listAdmin = _unitOfWork.AdminRepository.Get(
                    filter: s => s.Status == true && s.RoleId == 0,
                    includeProperties: "Role"
                ).ToList();
                var adminResponses = _mapper.Map<IEnumerable<AdminResponse>>(listAdmin);
                return adminResponses;
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                throw new CustomException.InternalServerErrorException("An error occurred during data processing.", ex);
            }
        }



        public async Task<AdminResponse> GetAdminById(long id)
        {
            try
            {

                var admin = _unitOfWork.AdminRepository.Get(
                    filter: a => a.Id == id && a.Status == true && a.RoleId == 1, includeProperties: "Role"
                ).FirstOrDefault();

                if (admin == null)
                {
                    throw new CustomException.DataNotFoundException("Admin not found."); 
                }

                var adminResponse = _mapper.Map<AdminResponse>(admin);
                return adminResponse;
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                throw new CustomException.InternalServerErrorException("An error occurred during data processing.", ex);
            }
        }



        public async Task<AdminResponse> CreateAdmin(AdminRequest adminRequest)
        {
            try
            {

                Authentication authentication = new(_configuration, _unitOfWork);

                bool usernameExists = _unitOfWork.AdminRepository.Exists(a => a.Username == adminRequest.UserName);
                if (usernameExists)
                {
                    throw new CustomException.InvalidDataException("Username already exists.");
                }

                var admin = _mapper.Map<Admin>(adminRequest);

                admin.Status = true;
                admin.RoleId = 1;

                _unitOfWork.AdminRepository.Insert(admin);
                _unitOfWork.Save();

                var adminResponse = _mapper.Map<AdminResponse>(admin);
                return adminResponse;
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                throw new CustomException.InternalServerErrorException("An error occurred during data processing.", ex);
            }
        }

        public async Task<AdminResponse> UpdateAdmin(long id, AdminRequest adminRequest)
        {
            try
            {
                Authentication authentication = new(_configuration, _unitOfWork);

                var existingAdmin = _unitOfWork.AdminRepository.GetByID(id);

                if (existingAdmin == null)
                {
                    throw new CustomException.DataNotFoundException("Admin not found.");
                }

                var admin = _mapper.Map(adminRequest, existingAdmin);

                _unitOfWork.AdminRepository.Update(existingAdmin);
                _unitOfWork.Save();

                var adminResponse = _mapper.Map<AdminResponse>(existingAdmin);
                return adminResponse;
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                throw new CustomException.InternalServerErrorException("An error occurred during data processing.", ex);
            }
        }

        public async Task<bool> DeleteAdmin(long id)
        {
            try
            {
                var accountId = Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
                if (string.IsNullOrEmpty(accountId))
                {
                    throw new CustomException.ForbbidenException("User ID claim invalid.");
                }

                if (!long.TryParse(accountId, out long Id))
                {
                    throw new CustomException.ForbbidenException("User ID claim invalid.");
                }

                var admin = _unitOfWork.AdminRepository.GetByID(id);
                if (admin == null)
                {
                    throw new CustomException.DataNotFoundException("Admin not found.");
                }

                admin.Status = false;
                _unitOfWork.AdminRepository.Update(admin);
                _unitOfWork.Save();

                return true;
            }
            catch (CustomException.InternalServerErrorException ex)
            {
                throw new CustomException.InternalServerErrorException("An error occurred during data processing.", ex);
            }
        }
    }
}
