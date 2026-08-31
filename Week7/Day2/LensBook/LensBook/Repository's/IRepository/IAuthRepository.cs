using LensBook.Models;

namespace LensBook.Repository_s.IRepository
{
    public interface IAuthRepository
    {
        Task<Customer?> GetCustomerByUserIdAsync(int userId);

        Task<Photographer?> GetPhotographerByUserIdAsync(int userId);

        Task AddCustomerAsync(Customer customer);

        Task AddRefreshTokenAsync(
            RefreshToken refreshToken);

        Task<RefreshToken?> GetRefreshTokenAsync(
            string token);

        Task RevokeRefreshTokenAsync(
            RefreshToken refreshToken);

        Task SaveChangesAsync();
    }
}
