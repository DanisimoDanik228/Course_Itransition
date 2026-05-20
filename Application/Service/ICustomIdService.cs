using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public interface ICustomIdService
    {
        IEnumerable<PartNameCustomId> GetAllPartCustomId();
        bool IsValidCustomId(string structCustomId, string customId, long sequence);
        string GenerateCustomId(string structCustomId, long sequence);
    }
}
