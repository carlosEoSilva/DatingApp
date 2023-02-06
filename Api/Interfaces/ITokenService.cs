
using Api.Entities;

namespace Api.Interfaces
{
    public interface ITokenService
    {
        //-interfaces não implementam lógica, ela contém apenas a assinatura dos métodos.
        Task<string> CreateToken(AppUser user);        
    }
}