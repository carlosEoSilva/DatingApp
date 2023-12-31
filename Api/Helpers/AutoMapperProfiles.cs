using Api.DTOs;
using Api.Entities;
using Api.Extensions;
using AutoMapper;

//-esta classe precisa ser incluída o 'ApplicationServiceExtensions.cs'.

namespace Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //-o primeiro parâmetro é o 'map from', e o segundo é o 'map to'.
            //-o 'ForMember' é para fazer o mapeamento de uma propriedade específica.
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, 
                    opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, 
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));                                  
            
            CreateMap<Photo, PhotoDto>();

            //-nao entendi a ordem das classes...
            // CreateMap<AppUser, MemberUpdateDto>();
            CreateMap<MemberUpdateDto, AppUser>();

            CreateMap<RegisterDto, AppUser>();

            CreateMap<Message, MessageDto>()
                .ForMember(d => d.SenderPhotUrl, o => o.MapFrom(s => s.Sender.Photos
                    .FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos
                    .FirstOrDefault(x => x.IsMain).Url));

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ?
                DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}