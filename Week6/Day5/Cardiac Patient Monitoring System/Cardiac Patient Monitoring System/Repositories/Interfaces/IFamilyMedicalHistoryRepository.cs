using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IFamilyMedicalHistoryRepository
    {
        Task<List<FamilyMedicalHistory>> GetByPatientIdAsync(int patientId);

        Task CreateRangeAsync(List<FamilyMedicalHistory> familyHistory);
    }
}
