using Sat.Recruitment.Model;
using Sat.Recruitment.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sat.Recruitment.Service
{
    public interface IUserService
    {
        Result CreateUser(User user);
    }
}
