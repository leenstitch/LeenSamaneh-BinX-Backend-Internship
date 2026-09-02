using LensBook.DATA;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LensBook.Repositories
{
    public class SessionTypeRepository : ISessionTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public SessionTypeRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

       
        // ADD

        public async Task<SessionType> AddAsync(
            SessionType sessionType)
        {
            await _context.SessionTypes.AddAsync(
                sessionType);

            return sessionType;
        }

        
        // GET BY ID
 
        public async Task<SessionType?> GetByIdAsync(
            int id)
        {
            return await _context.SessionTypes
                .FirstOrDefaultAsync(
                    s => s.SessionTypeId == id);
        }

        
        // GET ALL
      
        public async Task<IEnumerable<SessionType>> GetAllAsync()
        {
            return await _context.SessionTypes
                .ToListAsync();
        }

        // SAVE CHANGES

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}