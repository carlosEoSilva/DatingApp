using Api.Extensions;
using Api.DTOs;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Entities;
using Api.Helpers;

namespace Api.Controllers
{
    [Authorize]      
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;

        //-acesso ao banco de dados por meio da classe 'DataContext' que eu criei.
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _mapper= mapper;
            _photoService = photoService;
            _userRepository = userRepository;
        }

        [HttpGet]
        //-método que será executado quando for feito um 'get' no endpoint do controller.
        //-o retorno de um controller sempre deve ser do tipo 'ActionResult'.
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            /* 
            //-converter para 'List' as informações que o '_context' buscou na tabela 'Users'.
            var users= await _userRepository.GetUsersAsync();
            var usersToReturn= _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(usersToReturn);
            */

            var currentUser= await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername= currentUser.UserName;

            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender= currentUser.Gender == "male" ? "female" : "male";
            }

            var users= await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }

        //-fazer um 'get' passando parâmetros na url.
        //-o 'async Task<>' torna a função assíncrona, o retorno sempre deve ter o 'await'.
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            // var user= await _userRepository.GetUserByUsernameAsync(username);
            // return _mapper.Map<MemberDto>(user);

            var member= await _userRepository.GetMemberAsync(username);
            return member;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username= User.GetUsername();
            var user= await _userRepository.GetUserByUsernameAsync(username);

            if(user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);

            if(await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Failed to update user");
            }
        }

        //-quando é feito uma requisição 'post' o correto é retornar uma resposta '201'.
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user= await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null)
            {
                return NotFound();
            }

            var result= await _photoService.AddPhotoAsync(file);

            if(result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo= new Photo
            {
                Url= result.SecureUrl.AbsoluteUri,
                PublicId= result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain= true;
            }

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            {
                //-criar o recurso e enviar a resposta '201' com detalhes adicionais.
                return CreatedAtAction(
                    nameof(GetUser), new {username= user.UserName}, 
                    _mapper.Map<PhotoDto>(photo));
            }
            else
            {
                return BadRequest("Problem adding photo");
            }

        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user= await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();

            //-o parâmetro photoId é o Id da foto que foi selecionada pelo usuário para ser a principal.
            //-aqui estou verificando se a foto selecionada existe no banco.
            var photo= user.Photos.FirstOrDefault(x=> x.Id == photoId);
            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("this is already your main photo");

            var currentMain= user.Photos.FirstOrDefault(x => x.IsMain);

            if(currentMain != null) currentMain.IsMain = false;
            
            photo.IsMain= true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting the main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId){
            var user= await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo= user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot delete your main photo");

            if(photo.PublicId != null)
            {
                var result= await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting photo");
        }



    }
}