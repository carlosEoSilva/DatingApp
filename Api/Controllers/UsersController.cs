using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Authorize]      
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        //-acesso ao banco de dados por meio da classe 'DataContext' que eu criei.
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper= mapper;
            _userRepository= userRepository;
        }

        [HttpGet]
        //-método que será executado quando for feito um 'get' no endpoint do controller.
        //-o retorno de um controller sempre deve ser do tivo 'ActionResult'.
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            //-converter para 'List' as informações que o '_context' buscou na tabela 'Users'.
            // var users= await _userRepository.GetUsersAsync();
            // var usersToReturn= _mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);

            var users= await _userRepository.GetMembersAsync();
            return Ok(users);
        }

        //-fazer um 'get' passando parâmetros na url.
        //-o 'async Task<>' torna a função assíncrona, o retorno sempre deve ter o 'await'.
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            // var user= await _userRepository.GetUserByUsernameAsync(username);
            // return _mapper.Map<MemberDto>(user);

            return await _userRepository.GetMemberAsync(username);
        }



    }
}