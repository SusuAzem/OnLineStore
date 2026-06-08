using Core;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Business
{
    public interface IUserService
    {
        Task<User> HalfReg(List<Claim> claims);
    }
}
