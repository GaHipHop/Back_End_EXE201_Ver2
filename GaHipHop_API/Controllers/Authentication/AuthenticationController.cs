using CoreApiResponse;
using GaHipHop_Model.DTO.Request;
using GaHipHop_Service.Interfaces;
using GaHipHop_Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;

namespace GaHipHop_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _authenticationService.AuthorizeUser(loginRequest);
            if (result.Token != null)
            {
                return CustomResult("Login successful.", new { result.Token, LoginResponse = result.loginResponse });
            }
            else
            {
                return CustomResult("Invalid email or password.", HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] LoginGoogleRequest loginGoogleRequest)
        {
            var result = await _authenticationService.AuthorizeLoginGoogleUser(loginGoogleRequest);

            if (result.Token != null)
            {
                return CustomResult("Login successful.", new { result.Token, LoginResponse = result.loginResponse });
            }
            else
            {
                return CustomResult("Invalid email or password.", HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var (result, token) = await _authenticationService.GenerateAndSendOTPAsync(request.Email);
                if (result)
                {
                    return CustomResult("OTP has been sent to your email.", new { Token = token }, HttpStatusCode.OK);
                }
            
                else
                {
                    return CustomResult("Email not found.", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        /*[HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOTP([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var result = await _authenticationService.ResendOTPAsync(request.Email);
                if (result)
                {
                    return CustomResult("New OTP has been sent to your email.", HttpStatusCode.OK);
                }
                else
                {
                    return CustomResult("Unable to resend OTP. Email may not be registered or an OTP may not have been generated recently.", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }*/


        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var isOtpValid = await _authenticationService.VerifyOtpAsync(request.Email, request.Otp, request.Token);
            if (isOtpValid)
            {
                return CustomResult("OTP is valid.", HttpStatusCode.OK);
            }
            else
            {
                return CustomResult("Invalid OTP or token.", HttpStatusCode.BadRequest);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPasswordAsync(request.Email, request.NewPassword);
            if (result)
            {
                return CustomResult("Password has been reset successfully.", HttpStatusCode.OK);
            }
            else
            {
                return CustomResult("Failed to reset the password.", HttpStatusCode.InternalServerError);
            }
        }




    }
}
