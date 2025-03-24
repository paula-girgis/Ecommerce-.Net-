using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Repositry.Data;

namespace WebApplication1.Api.Middlewares
{
    public class TokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, EcommerceContext dbContext)

        {
            if (context.Request.Path.StartsWithSegments("/user/login") ||
       context.Request.Path.StartsWithSegments("/user/register") ||
       context.Request.Path.StartsWithSegments("/user/EmailConfirmation") ||
       context.Request.Path.StartsWithSegments("/user/Reset-Password") ||
       context.Request.Path.StartsWithSegments("/user/Forget-Password"))
            {
                await _next(context);
                return;
            }
            var tokenValue = context.Request.Headers["tk"].FirstOrDefault();

            if (string.IsNullOrEmpty(tokenValue))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Token is missing");
                return;
            }

            var token = await dbContext.Tokens.FirstOrDefaultAsync(t => t.token == tokenValue && t.isValid);
            if (token == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Invalid Token");
                return;
            }

            await _next(context);
        }
    }
}
