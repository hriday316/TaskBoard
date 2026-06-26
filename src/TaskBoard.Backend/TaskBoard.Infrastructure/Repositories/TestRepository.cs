using TaskBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;


namespace TaskBoard.Infrastructure.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;

        public TestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Test>> GetAllAsync()
        {
            return await _context.Tests.AsNoTracking().ToListAsync();
        }

        public async Task<Test?> GetByIdAsync(int id)
        {
            return await _context.Tests.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> CreateAsync(Test entity)
        {
            _context.Tests.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(Test entity)
        {
            _context.Tests.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var obj = await GetByIdAsync(id);
            if (obj == null)
                return false;

            _context.Tests.Remove(obj);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
