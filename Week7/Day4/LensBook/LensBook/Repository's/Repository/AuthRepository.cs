
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

        //get customerId by user id
        public async Task<Customer?> GetCustomerByUserIdAsync(
            int userId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(
                    c => c.UserId == userId);
        }

        //get photographerId by user id
        public async Task<Photographer?> GetPhotographerByUserIdAsync(
            int userId)
        {
            return await _context.Photographers
                .FirstOrDefaultAsync(
                    p => p.UserId == userId);
        }


        //add customer to the database
        public async Task AddCustomerAsync(
            Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        //add refresh token to the database
        public async Task AddRefreshTokenAsync(
            RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(
                refreshToken);
        }

        //get refresh token by token string
        public async Task<RefreshToken?> GetRefreshTokenAsync(
            string token)
        {
            return await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(
                    r => r.Token == token);
        }

        //revoke refresh token
        public async Task RevokeRefreshTokenAsync(
            RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;

            _context.RefreshTokens.Update(
                refreshToken);

            await Task.CompletedTask;
        }

        //save changes to the database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
