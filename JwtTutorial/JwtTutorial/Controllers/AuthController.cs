using JwtTutorial.DTOs;
using JwtTutorial.Entities;
using JwtTutorial.Models;
using JwtTutorial.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO req)
        {
            var actionResult = await _authService.Register(req);
            var res = actionResult.Value;
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<bool>>> Login(UserDTO req)
        {
            var actionResult = await _authService.Login(req);
            var res = actionResult.Value;
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
        [Authorize]
        [HttpGet]
        public ActionResult<object> GetInfoByClaim()
        {
            var res = _authService.GetInfoByClaim();
            return Ok(res);
        }
    }
}
