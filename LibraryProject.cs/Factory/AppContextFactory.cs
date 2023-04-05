using Library.Shared.Api.ApplicationContext;
using System.Security.Claims;

namespace Library.API.Factory
{
    public class AppContextFactory
    {
        public static ApplicationContext Create(IServiceProvider ctx)
        {
            var httpContext = ctx.GetService<IHttpContextAccessor>()?.HttpContext;

            return Create(httpContext);
        }
        public static ApplicationContext Create(HttpContext? httpContext)
        {
            var remoteIpAddress = httpContext?.Connection?.RemoteIpAddress?.ToString();

            var header = httpContext?.Request.Headers["Authorization"];
            var auth = header?.FirstOrDefault()?.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var token = auth?.LastOrDefault();

            if (httpContext != null && !string.IsNullOrEmpty(token))
            {
                var idClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                _ = int.TryParse(idClaim?.Value, out var id);

                var rolesClaim = httpContext.User.FindFirst(ClaimTypes.Role);
                var role = rolesClaim?.Value;
                if (role == null) role = "User";
                return new ApplicationContext(id, role, token, remoteIpAddress);
            }

            return new ApplicationContext(remoteIpAddress);
        }
    }
}
