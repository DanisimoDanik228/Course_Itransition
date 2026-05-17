using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public interface IAuthenticationService
    {
        Task<bool> MayEditInventory(string userId, long inventoryId);
        string MyId();
    }
}
