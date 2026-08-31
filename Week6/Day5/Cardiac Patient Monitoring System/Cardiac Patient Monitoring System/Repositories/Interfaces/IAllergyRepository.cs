using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    
        public interface IAllergyRepository
        {
            Task<List<Allergy>> GetByPatientIdAsync(int patientId);

            Task CreateRangeAsync(List<Allergy> allergies);
        }

    
}
