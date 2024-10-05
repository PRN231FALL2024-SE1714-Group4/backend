using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Filter
{
    public class JwtAuthorizeAttribute: TypeFilterAttribute
    {
        public JwtAuthorizeAttribute(params string[] roles) : base(typeof(JwtAuthorizeFilter))
        {
            Arguments = new object[] { roles };
        }


    }
}
