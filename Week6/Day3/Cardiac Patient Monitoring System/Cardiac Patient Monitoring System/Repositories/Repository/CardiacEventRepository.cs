using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class CardiacEventRepository
        : ICardiacEventRepository
    {
        private readonly ApplicationDbContext _context;

        public CardiacEventRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CardiacEvent?> GetByIdAsync(int id)
        {
            return await _context.CardiacEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.CardiacEventId == id);
        }

        public async Task<IEnumerable<CardiacEvent>>
            GetPreviousEventsAsync(
                int patientId,
                DateTime startDate,
                DateTime eventDate)
        {
            return await _context.CardiacEvents
                .AsNoTracking()
                .Where(x =>
                    x.PatientId == patientId &&
                    x.EventDate >= startDate &&
                    x.EventDate < eventDate)
                .OrderBy(x => x.EventDate)
                .ToListAsync();
        }
    }
}