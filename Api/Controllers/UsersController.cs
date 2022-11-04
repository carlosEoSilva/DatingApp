using Api.Data;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    //-a rota deve começar com 'api/' seguida do nome do 'controller'.
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        //-acesso ao banco de dados por meio da classe 'DataContext' que eu criei.
        public UsersController(DataContext context)
        {
            _context= context;
        }



        [HttpGet]
        //-método que será executado quando for feito um 'get' no endpoint do controller.
        //-o retorno de um controller sempre deve ser do tivo 'ActionResult'.
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            //-converter para 'List' as informações que o '_context' buscou na tabela 'Users'.
            var users= _context.Users.ToListAsync();
            return await users;
        }

        //-fazer um 'get' passando parâmetros na url.
        //-o 'async Task<>' torna a função assíncrona, o retorno sempre deve ter o 'await'.
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user= _context.Users.FindAsync(id);
            return await user;
        }



    }
}