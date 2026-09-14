using LensBook.Models;

namespace LensBook.Repository_s.IRepository
{
    public interface ISessionTypeRepository
    {
        // add a new session type
        Task<SessionType> AddAsync(
           SessionType sessionType);

        // get session type by id
        Task<SessionType?> GetByIdAsync(
            int id);

        // get all session types
        Task<IEnumerable<SessionType>> GetAllAsync();

        // save changes to the database
        Task SaveChangesAsync();
    }
}