using Dan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Application.Contract
{
    public interface IMotoRepository
    {
        Task<IEnumerable<Moto>> GetAllAsync();
        Task<Moto> GetAsync(int id);
        Task AddAsync(Moto brand);
        Task UpdateAsync(Moto brand);
        Task DeleteAsync(int id);
    }
}
