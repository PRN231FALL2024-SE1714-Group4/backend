using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Response
{
    public class UserResponse
    {
        public String Token {  get; set; }
        public String FullName { get; set; }
        public String Email { get; set; }
        public String Role { get; set; }

        public UserResponse(string token, string fullName, string email, string role)
        {
            Token = token;
            FullName = fullName;
            Email = email;
            Role = role;
        }
    }
}
