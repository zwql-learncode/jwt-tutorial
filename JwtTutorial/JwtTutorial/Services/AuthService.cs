using JwtTutorial.Context;
using JwtTutorial.DTOs;
using JwtTutorial.Entities;
using JwtTutorial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtTutorial.Services
{
    public class AuthService : IAuthService
    {
        private DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserDTO userDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDTO.UserName);
            if (user == null)
            {
                return new ServiceResponse<string>()
                {
                    Success = false,
                    Message = "Username is not correct!"
                };
            }
            if(!VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new ServiceResponse<string>()
                {
                    Success = false,
                    Message = "Password is not correct!"
                };
            }
            return new ServiceResponse<string>()
            {
                Data = GenetareToken(user),
                Message = "Login Successfully"
            };
        }

        public async Task<ActionResult<ServiceResponse<User>>> Register(UserDTO userDTO)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDTO.UserName);
            if (dbUser != null)
            {
                return new ServiceResponse<User>()
                {
                    Data = null,
                    Success = false,
                    Message = "Username has already exist!"
                };
            }
            CreatePasswordHash(userDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User()
            {
                Username = userDTO.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new ServiceResponse<User>()
            {
                Data = user,
                Message = "Register Successfully"
            };
        }
        private void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            var hmac = new HMACSHA512(passwordSalt);
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }

        private string GenetareToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetting:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
