using Api.DTOs;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Api.Extensions;
using Api.Entities;
using Api.Helpers;

namespace Api.Controllers
{
    public class MessagesController:BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _userRepository= userRepository;
            _messageRepository= messageRepository;
            _mapper= mapper;
        }

        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username= User.GetUsername();

            if(username == createMessageDto.RecipientUsername.ToLower())
            {
                return BadRequest("You cannot send messages to yourself");
            }

            var sender= await _userRepository.GetUserByUsernameAsync(username);
            var recipient= await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if(recipient == null) return NotFound();

            var message= new Message
            {
                SenderId= sender.Id,
                RecipientId= recipient.Id,
                SenderUsername= sender.UserName,
                RecipientUsername= recipient.UserName,
                Content= createMessageDto.Content
            };

            _messageRepository.AddMessage(message);

            if(await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams)
        {
            messageParams.Username= User.GetUsername();

            var messages= await _messageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));    
        
            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUserName= User.GetUsername();

            return Ok(await _messageRepository.GetMessageThread(currentUserName, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username= User.GetUsername();
            var message= await _messageRepository.GetMessage(id);

            if(message.SenderUsername == username) 
                message.SenderDeleted= true;
            
            if(message.RecipientUsername == username)
                message.RecipienteDeleted= true;

            if(message.SenderDeleted && message.RecipienteDeleted)
            {
                _messageRepository.DeleteMessage(message);
            }

            if(await _messageRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting the message!");
        }
    }
}