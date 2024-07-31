using GaHipHop_Model.DTO.Request;
using GaHipHop_Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaHipHop_Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<(string Token, LoginResponse loginResponse)> AuthorizeUser(LoginRequest loginRequest);

        Task<(string Token, LoginResponse loginResponse)> AuthorizeLoginGoogleUser(LoginGoogleRequest loginGoogleRequest);

        Task<(bool IsSuccess, string Token)> GenerateAndSendOTPAsync(string email);

       /* Task<bool> ResendOTPAsync(string email);*/

        Task<bool> VerifyOtpAsync(string email, string otp, string token);

        Task<bool> ResetPasswordAsync(string email, string newPassword);
    }
}
