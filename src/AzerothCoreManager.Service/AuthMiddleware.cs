



using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AzerothCoreManager.Service
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecuritySettings _securitySettings;

        public AuthMiddleware(RequestDelegate next, IOptions<SecuritySettings> securitySettings)
        {
            _next = next;
            _securitySettings = securitySettings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for health check and open API endpoints
            if (context.Request.Path == "/health" ||
                context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/api-docs"))
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var token = authHeader["Bearer ".Length..].Trim();
            if (token != _securitySettings.ApiToken)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Forbidden");
                return;
            }

            await _next(context);
        }
    }
}



