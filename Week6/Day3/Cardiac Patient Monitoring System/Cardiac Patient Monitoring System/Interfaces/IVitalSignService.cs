// This interface defines the operations provided by the Vital Sign service.
// It handles retrieving, creating, filtering, updating, deleting,
// and comparing patient vital-sign records.

using Cardiac_Patient_Monitoring_System.DTO_S.Paginat;
using Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IVitalSignService
    {
        // Returns a vital-sign record by its ID.
        Task<VitalSignResponseDto?> GetByIdAsync(int id);

        // Returns vital-sign records belonging to a specific patient.
        Task<IEnumerable<VitalSignResponseDto>>GetByPatientIdAsync(int patientId);

        // Returns all vital-sign records.
        // it was for week 5 i implement a new interface for week 6
        //Task<IEnumerable<VitalSignResponseDto>>  GetAllAsync();
        Task<PaginatedResponseDto<VitalSignResponseDto>> GetAllAsync(VitalSignQueryDto query);

        // Creates a vital-sign record for the patient linked
        // to the authenticated user.
        Task<VitalSignResponseDto?>CreateAsync(int userId, CreateVitalSignDto dto);

        // Returns vital-sign records belonging to the authenticated patient.
        Task<IEnumerable<VitalSignResponseDto>> GetMyVitalSignsAsync(int userId);

        // Filters vital-sign records using patient information.
        Task<IEnumerable<VitalSignResponseDto>> FilterAsync(VitalSignFilterDto filter);

        // Updates an existing vital-sign record.
        Task<bool> UpdateAsync(int id, UpdateVitalSignDto dto);

        // Deletes an existing vital-sign record.
        Task<bool> DeleteAsync(int id);

        // Compares the patient's two latest vital-sign records.
        Task<VitalSignComparisonDto?> CompareLatestTwoAsync(int userId);

        // Compares vital-sign records from two selected dates.
        Task<VitalSignDateComparisonDto?>CompareByDatesAsync(
                int userId,
                DateTime firstDate,
                DateTime secondDate);
    }
}