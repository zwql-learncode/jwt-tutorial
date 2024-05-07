using JwtTutorial.DTOs;
using JwtTutorial.Entities;
using JwtTutorial.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtTutorial.Services
{
    public interface IAuthService
    {
        Task<ActionResult<ServiceResponse<User>>> Register(UserDTO userDTO);
        Task<ActionResult<ServiceResponse<string>>> Login(UserDTO userDTO);
        object GetInfoByClaim();
        Task<ActionResult<ServiceResponse<string>>> CheckRefreshToken(string username, string refreshToken);
    }
}
