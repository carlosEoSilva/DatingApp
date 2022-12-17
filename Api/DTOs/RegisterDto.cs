
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.DTOs
{
    public class RegisterDto
    {
        [Required] public string Username { get; set; }
        [Required] public string KnownAs { get; set; }
        [Required] public string Gender { get; set; }
        
        //-para que '[Required]' funcionar com 'DateOnly' preciso usar o '?'.
        //-o 'DateOnly' estava dando erro então mudei para 'DateTime.'
        [Required] public DateTime? DateOfBirth { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }

        [Required]
        //-validação extra, a senha deve ter no mínimo 4 e no máximo 8 caracteres.
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}