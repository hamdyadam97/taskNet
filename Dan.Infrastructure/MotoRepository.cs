using Dan.Application.Contract;
using Dan.Context;
using Dan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Infrastructure
{
    public class MotoRepository : IMotoRepository
    {
        private readonly DanDbContext _context;

        public MotoRepository(DanDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Moto brand)
        {
            _context.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
           
        }

        public async Task<IEnumerable<Moto>> GetAllAsync()
        {
            return [];
        }

        public async Task<Moto> GetAsync(int id)
        {
            return new Moto();
        }

        public async Task UpdateAsync(Moto brand)
        {
            _context.Update(brand);
            await _context.SaveChangesAsync();
        }
    }
}
