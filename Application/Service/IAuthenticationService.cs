using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public interface IAuthenticationService
    {
        Task<bool> MayEditInventory(string userId, long inventoryId);
        Task<bool> MayDropAndCreateInventory(string userId, long inventoryId);
        Task<bool> MayDropAndCreateEditor(string userId, long inventoryId);
        Task<bool> IsAdmin(string userId);
        string MyId();
    }
}
