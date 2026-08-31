using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IEmergencyMedicalInformationRepository
    {
        Task<EmergencyMedicalInformation?> GetByPatientIdAsync(
            
            int patientId);
    }
}
