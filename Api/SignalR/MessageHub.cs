using Api.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Api.Extensions;
using Api.DTOs;
using Api.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Api.SignalR
{
    [Authorize]
    public class MessageHub:Hub
    {
        public IMessageRepository _messageRepository { get; }
        public IUserRepository _userRepository { get; }
        private IMapper _mapper;

        public MessageHub(
            IMessageRepository messageRepository, 
            IUserRepository userRepository,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _userRepository= userRepository;
            _mapper= mapper;
        }

        public override async Task OnConnectedAsync()
        {
            
            var httpContext= Context.GetHttpContext();
            var otherUser= httpContext.Request.Query["user"];
            var groupName= GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages= await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username= Context.User.GetUsername();

            if(username == createMessageDto.RecipientUsername.ToLower())
            {
                throw new HubException("You cannot send messages to yourself");
            }

            var sender= await _userRepository.GetUserByUsernameAsync(username);
            var recipient= await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if(recipient == null) throw new HubException("User Not found");

            var message= new Message
            {
                SenderId= sender.Id,
                RecipientId= recipient.Id,
                SenderUsername= sender.UserName,
                RecipientUsername= recipient.UserName,
                Content= createMessageDto.Content
            };

            _messageRepository.AddMessage(message);

            if(await _messageRepository.SaveAllAsync())
            {
                var group= GetGroupName(sender.UserName, recipient.UserName);
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare= string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}