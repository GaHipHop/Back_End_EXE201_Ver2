using AutoMapper;
using GaHipHop_Model.DTO.Request;
using GaHipHop_Model.DTO.Response;
using GaHipHop_Repository.Repository;
using GaHipHop_Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace GaHipHop_Service.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly Dictionary<string, string> _otpStore = new Dictionary<string, string>();

        public AuthenticationService(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<(string Token, LoginResponse loginResponse)> AuthorizeUser(LoginRequest loginRequest)
        {

            Authentication authentication = new(_configuration, _unitOfWork);

            var member = _unitOfWork.AdminRepository
                .Get(filter: a => a.Username == loginRequest.UserName && a.Status == true).FirstOrDefault();
            if (member != null && authentication.VerifyPassword(loginRequest.Password, member.Password))
            {
                string token = authentication.GenerateToken(member);
                var adminResponse = _mapper.Map<LoginResponse>(member);
                return (token, adminResponse);
            }
            return (null, null);
        }

        public async Task<(string Token, LoginResponse loginResponse)> AuthorizeLoginGoogleUser(LoginGoogleRequest loginGoogleRequest)
        {
            var authentication = new Authentication(_configuration, _unitOfWork);

            var admin = _unitOfWork.AdminRepository.Get(a => a.Email == loginGoogleRequest.Email).FirstOrDefault();

            if (admin != null)
            {
                var token = authentication.GenerateToken(admin);

                var adminResponse = _mapper.Map<LoginResponse>(admin);

                return (token, adminResponse);
            }

            return (null, null);
        }

        public async Task<(bool IsSuccess, string Token)> GenerateAndSendOTPAsync(string email)
        {
            var user = _unitOfWork.AdminRepository.Get(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                return (false, null);
            }

            // Generate a new OTP
            var otp = new Random().Next(100000, 999999).ToString();

            // Create JWT token with OTP and Email claims
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim("Email", email),
            new Claim("OTP", otp)
        }),
                Expires = DateTime.UtcNow.AddSeconds(60), // Adjust the expiration time as needed
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            // Store the OTP in memory with an expiration time
            _otpStore[email] = otp;

            var subject = "Your OTP for password reset";
            var message = $"Your OTP code is {otp}. It will expire in 60s.";

            await _emailService.SendEmailAsync(email, subject, message);
            return (true, encryptedToken);
        }


       /* public async Task<(bool IsSuccess, string Token)> ResendOTPAsync(string email)
        {
            *//*if (!_otpStore.ContainsKey(email))
            {
                return false; // No OTP found for the email, can't resend
            }

            // Invalidate the old OTP (remove it from the store)
            _otpStore.Remove(email);

            // Call the existing method to generate and send a new OTP
            return await GenerateAndSendOTPAsync(email);*//*
        }*/


        public async Task<bool> VerifyOtpAsync(string email, string otp, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var tokenEmail = jwtToken.Claims.First(x => x.Type == "Email").Value;
                var tokenOtp = jwtToken.Claims.First(x => x.Type == "OTP").Value;

                return email == tokenEmail && otp == tokenOtp;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = _unitOfWork.AdminRepository.Get(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                return false;
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _unitOfWork.Save();
            return true;
        }




    }
}
