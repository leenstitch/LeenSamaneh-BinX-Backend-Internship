using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class CardiacEventRepository : ICardiacEventRepository
    {
        private readonly ApplicationDbContext _context;

        public CardiacEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        // Returns a cardiac event by its ID.
        public async Task<CardiacEvent?> GetByIdAsync(int id)
        {
            return await _context.CardiacEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.CardiacEventId == id);
        }


        // Returns previous cardiac events for a patient 
        // within the specified date range.
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
                .OrderByDescending(x => x.EventDate)
                .ToListAsync();
        }

        // Creates a new cardiac event and saves it to the database.
        public async Task<CardiacEvent> AddAsync(
      CardiacEvent cardiacEvent)
        {
            await _context.CardiacEvents.AddAsync(
                cardiacEvent);

            await _context.SaveChangesAsync();

            return cardiacEvent;
        }

        // Finds the PatientId associated with the specified UserId.
        public async Task<int?> GetPatientIdByUserIdAsync(
    int userId)
        {
            return await _context.Patients
                .Where(p => p.UserId == userId)
                .Select(p => (int?)p.PatientId)
                .FirstOrDefaultAsync();
        }
    }
    }
