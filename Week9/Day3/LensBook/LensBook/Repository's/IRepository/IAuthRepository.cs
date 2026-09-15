using LensBook.Models;

namespace LensBook.Repository_s.IRepository
{
    public interface IAuthRepository
    {
        //get customerId by user id
        Task<Customer?> GetCustomerByUserIdAsync(int userId);

        //Get photographer by user id
        Task<Photographer?> GetPhotographerByUserIdAsync(int userId);

        // create a new customer
        Task AddCustomerAsync(Customer customer);


        //create refresh token
        Task AddRefreshTokenAsync(
            RefreshToken refreshToken);

        //get refresh token by token string
        Task<RefreshToken?> GetRefreshTokenAsync(
            string token);


        // revoke refresh token
        Task RevokeRefreshTokenAsync(
            RefreshToken refreshToken);

        //save changes to the database
        Task SaveChangesAsync();
    }
}
