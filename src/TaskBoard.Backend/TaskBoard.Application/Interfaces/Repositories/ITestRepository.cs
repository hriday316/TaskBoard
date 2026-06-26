
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories
{
    public interface ITestRepository
    {
        Task<List<Test>> GetAllAsync();
        Task<Test?> GetByIdAsync(int id);
        Task<int> CreateAsync(Test entity);
        Task<bool> UpdateAsync(Test entity);
        Task<bool> DeleteAsync(int id);
    }
}
