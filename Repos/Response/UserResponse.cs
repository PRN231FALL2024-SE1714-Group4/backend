using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Response
{
    public class UserResponse
    {
        public String Token;
        public String FullName;
        public String Email;
        public String Role;

        public UserResponse(string token, string fullName, string email, string role)
        {
            Token = token;
            FullName = fullName;
            Email = email;
            Role = role;
        }
    }
}
