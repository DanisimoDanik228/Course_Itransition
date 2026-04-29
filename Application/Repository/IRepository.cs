using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<Inventory?> AddAsync(Inventory item);
    }
}
