
using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        //-validação extra, a senha deve ter no mínimo 4 e no máximo 8 caracteres.
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}