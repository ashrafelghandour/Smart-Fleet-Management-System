using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagementSystem.Application.User;

namespace FleetManagementSystem.Application.Interface
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }
}