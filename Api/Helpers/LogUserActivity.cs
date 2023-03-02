using Api.Extensions;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Helpers
{
    //-1
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext= await next();

            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId= resultContext.HttpContext.User.GetUsername();
            var uow= resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            var user= await uow.UserRepository.GetUserByUsernameAsync(userId);
            user.LastActive= DateTime.UtcNow;
            await uow.Complete();
        }
    }
}