using PassManager.Domain.Entities;

namespace PassManager.Application.Interfaces
{
    public interface IPasswordRepository
    {
        Task<IEnumerable<PasswordRecord>> GetPasswordsAsync();
        Task AddPasswordAsync(PasswordRecord passwordRecord);
        Task<bool> PasswordExistsAsync(string name);
        Task DeletePasswordAsync(int id);
        Task<PasswordRecord?> GetPasswordByIdAsync(int id);
    }
}
