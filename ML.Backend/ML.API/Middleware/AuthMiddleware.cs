using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthMiddleware> _logger;

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;

        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
        if (string.IsNullOrEmpty(jwtSecret))
        {
            _logger.LogError("JWT_SECRET is not configured.");
            throw new ArgumentNullException(nameof(jwtSecret), "JWT_SECRET is not configured.");
        }
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path.Value.ToLower();
        if (context.Request.Method == HttpMethods.Options)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Content-Type");
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return;
        }

        if (path.Contains("/api/auth/login") ||
            path.Contains("/api/auth/register") ||
            path.Contains("/swagger"))
        {
            await _next(context);
            return;
        }

        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            if (!AttachUserToContext(context, token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }

    private bool AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            _logger.LogInformation("Validating token: {Token}", token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            context.Items["UserId"] = userId;
            _logger.LogInformation("Token validated successfully. UserId: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token validation failed");
            return false;
        }
    }
}