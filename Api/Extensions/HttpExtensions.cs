using System.Text.Json;
using Api.Helpers;

namespace Api.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOptions= new JsonSerializerOptions{PropertyNamingPolicy= JsonNamingPolicy.CamelCase};
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOptions));
            
            //-permitir explicitamente o 'custom header' para não dar erro de 'cors'
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}