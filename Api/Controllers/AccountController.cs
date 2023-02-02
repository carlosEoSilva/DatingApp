using System.Security.Cryptography;
using System.Text;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            //-acessar o banco 
            _userManager = userManager;

            //-acessar o token
            _tokenService = tokenService;
            
            //-???
            _mapper = mapper;
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

            //-aqui é criado uma nova instância de 'AppUser' usando as informações do 'registerDto'.
            var user= _mapper.Map<AppUser>(registerDto);

            //-não preciso mais desta parte poque a instância de 'AppUser' vai ser criada usando o '_mapper'.
            // var user= new AppUser
            // {
            //     UserName= registerDto.Username,
            //     PasswordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //     PasswordSalt= hmac.Key
            // };

            user.UserName= registerDto.Username.ToLower();
            user.Gender= registerDto.Gender.ToLower();

            var result= await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded) return BadRequest(result.Errors);
            
            return new UserDto
            {
                Username= user.UserName,
                Token= _tokenService.CreateToken(user),
                KnownAs= user.KnownAs,
                Gender= user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //-foi usado o 'SingleOrDefaultAsync' porque 'UserName' não é chave primária.
            //-o '.Include' é para fazer a junção com as tabelas relacionadas e adicionar o resultado na resposta da query.
            var user= await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if(user == null)
            {
                return Unauthorized("Invalid username");
            }

            var result= await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if(!result) return Unauthorized("Invalid password");
            
            return new UserDto
            {
                Username= user.UserName,
                Token= _tokenService.CreateToken(user),
                PhotoUrl= user.Photos.FirstOrDefault(x=> x.IsMain)?.Url,
                KnownAs= user.KnownAs,
                Gender= user.Gender
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }


    }

}