using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Filter
{
    public class JwtAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;

        public JwtAuthorizeFilter(string[] roles)
        {
            _roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Retrieve the token from the Authorization header
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                // If no token is present, return Unauthorized (401)
                context.Result = new JsonResult(new { message = "Unauthorized: No token provided" }) { StatusCode = 401 };
                return;
            }

            var jwtSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validate the token
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Optionally reduce clock skew
                }, out SecurityToken validatedToken);

                // Token is valid, now extract the claims
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;
                var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
                var userRoles = jwtToken.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();

                // Attach the ClaimsPrincipal (user) to the current request context
                var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "Jwt");
                context.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

                // Check if the user has the required roles before assigning the ClaimsPrincipal to the HttpContext
                if (_roles.Length > 0 && !userRoles.Any(role => _roles.Contains(role)))
                {
                    context.Result = new ForbidResult(); // 403
                    return;
                }
            }
            catch (SecurityTokenException ex)
            {
                // Token is invalid, return Unauthorized (401)
                context.Result = new JsonResult(new { message = "Unauthorized: Invalid token", error = ex.Message }) { StatusCode = 401 };
                return;
            }
            catch (Exception ex)
            {
                // Any other failure, return BadRequest (400)
                context.Result = new JsonResult(new { message = "BadRequest: Token validation failed", error = ex.Message }) { StatusCode = 400 };
                return;
            }

            // Check if the user is authenticated
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult(); // 401
                return;
            }
        }


    }
}
