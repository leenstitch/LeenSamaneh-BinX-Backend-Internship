
using LensBook.DATA;
using LensBook.Models;

using LensBook.Repository_s.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LensBook.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerByUserIdAsync(
            int userId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(
                    c => c.UserId == userId);
        }

        public async Task<Photographer?> GetPhotographerByUserIdAsync(
            int userId)
        {
            return await _context.Photographers
                .FirstOrDefaultAsync(
                    p => p.UserId == userId);
        }

        public async Task AddCustomerAsync(
            Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task AddRefreshTokenAsync(
            RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(
                refreshToken);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(
            string token)
        {
            return await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(
                    r => r.Token == token);
        }

        public async Task RevokeRefreshTokenAsync(
            RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;

            _context.RefreshTokens.Update(
                refreshToken);

            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
