using Application.Repository;
using Application.Service;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Infrastructure.Service.Service
{
    public class Service : IService
    {
        private readonly IRepository _repository;
        public Service(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<Inventory?> AddAsync(Inventory item)
        {
            return await _repository.AddAsync(item);
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
