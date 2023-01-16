using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var repo= resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user= await repo.GetUserByUsernameAsync(userId);
            user.LastActive= DateTime.UtcNow;
            await repo.SaveAllAsync();
        }
    }
}