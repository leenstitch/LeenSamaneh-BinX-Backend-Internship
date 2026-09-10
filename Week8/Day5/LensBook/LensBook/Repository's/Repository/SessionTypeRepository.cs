using System.Text.Json;
using LensBook.DATA;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace LensBook.Repositories
{
    public class SessionTypeRepository : ISessionTypeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        public SessionTypeRepository(
            ApplicationDbContext context,
            IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }


        // ADD

        public async Task<SessionType> AddAsync(
      SessionType sessionType)
        {
            await _context.SessionTypes.AddAsync(
                sessionType);
            // Invalidate the cache for all session types
            await _cache.RemoveAsync("sessiontypes:all");

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

        public async Task<IEnumerable<SessionType>> GetAllAsync()
        {

            // GET ALL SESSION TYPES WITH CACHING
            const string cacheKey = "sessiontypes:all";

            var cachedData =
                await _cache.GetStringAsync(cacheKey);

            // If cached data exists, deserialize and return it
            if (cachedData is not null)
            {
                return JsonSerializer.Deserialize<IEnumerable<SessionType>>(
                           cachedData)
                       ?? Enumerable.Empty<SessionType>();
            }

            var sessionTypes =
                await _context.SessionTypes
                    .ToListAsync();

            // Cache the data for future requests
            var serializedData =
                JsonSerializer.Serialize(sessionTypes);

            // Set cache options with an absolute expiration of 10 minutes
            await _cache.SetStringAsync(
               cacheKey,
               serializedData,
     new DistributedCacheEntryOptions
     {
         AbsoluteExpirationRelativeToNow =
             TimeSpan.FromMinutes(10)
     });

            return sessionTypes;
        }

        // SAVE CHANGES

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}