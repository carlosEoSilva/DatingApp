using System.Security.Cryptography;
using System.Text;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context= context;
            _tokenService= tokenService;
        }

        [HttpPost("register")]
        //-quando os parâmetros são passados no 'body' da requisição, a função espera recebê-los como objetos.
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if(await UserExists(registerDto.Username))
            {
                //-o tipo 'ActionResult' dá acesso aos objetos com código http, o 'BadRequest' é um deles.
                return BadRequest("User already taken!");
            }

            using var hmac= new HMACSHA512();

            var user= new AppUser
            {
                UserName= registerDto.Username,
                PasswordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt= hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return new UserDto
            {
                Username= user.UserName,
                Token= _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //-foi usado o 'SingleOrDefaultAsync' porque 'UserName' não é chave primária.
            var user= await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if(user == null)
            {
                return Unauthorized("Invalid username");
            }

            using var hmac= new HMACSHA512(user.PasswordSalt);
            
            var computedHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i= 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password");
                }
            }
            return new UserDto
            {
                Username= user.UserName,
                Token= _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }


    }

}