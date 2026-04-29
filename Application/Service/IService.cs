using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public interface IService
    {
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<Inventory?> AddAsync(Inventory item);
    }
}
