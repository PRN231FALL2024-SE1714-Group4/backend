using BOs;
using BOs.DTOS;
using Repos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IUserService
    {
        UserResponse registerAccount(UserRegisterRequest request);
        UserResponse Login(UserLoginRequest request);
    }
}
