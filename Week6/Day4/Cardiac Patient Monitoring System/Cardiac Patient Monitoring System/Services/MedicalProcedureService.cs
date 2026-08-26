using Cardiac_Patient_Monitoring_System.DTO_S.MedicalProcedureDto_s;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Services.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class MedicalProcedureService
        : IMedicalProcedureService
    {
        private readonly IMedicalProcedureRepository _repository;

        public MedicalProcedureService(
            IMedicalProcedureRepository repository)
        {
            _repository = repository;
        }


        // Retrieves medical procedures for a specific patient within a date range.
        public async Task<IEnumerable<MedicalProcedureResponseDto>>
            GetByPatientAndDateRangeAsync(
                int patientId,
                DateTime startDate,
                DateTime eventDate)
        {
            var results =
                await _repository
                    .GetByPatientAndDateRangeAsync(
                        patientId,
                        startDate,
                        eventDate);

            return results.Select(x =>
                new MedicalProcedureResponseDto
                {
                    ProcedureId =
                        x.ProcedureId,

                    PatientId =
                        x.PatientId,

                    ProcedureName =
                        x.ProcedureName,

                    ProcedureDate =
                        x.ProcedureDate,

                    HospitalName =
                        x.HospitalName,

                    Reason =
                        x.Reason,

                    Outcome =
                        x.Outcome,

                    Notes =
                        x.Notes
                });
        }


        // Creates a new medical-procedure record for the authenticated patient.
        public async Task<MedicalProcedureResponseDto?>
 CreateAsync(
     int userId,
     CreateMedicalProcedureDto dto)
        {
            var patientId =
                await _repository
                    .GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
            {
                return null;
            }

            var procedure = new MedicalProcedure
            {
                PatientId =
                    patientId.Value,

                DoctorId =
                    dto.DoctorId,

                ProcedureName =
                    dto.ProcedureName,

                ProcedureDate =
                    dto.ProcedureDate,

                HospitalName =
                    dto.HospitalName,

                Reason =
                    dto.Reason,

                Outcome =
                    dto.Outcome,

                Notes =
                    dto.Notes,

                CreatedAt =
                    DateTime.UtcNow,

                UpdatedAt =
                    DateTime.UtcNow
            };

            var createdProcedure =
                await _repository
                    .AddAsync(procedure);

            return new MedicalProcedureResponseDto
            {
                ProcedureId =
                    createdProcedure.ProcedureId,

                PatientId =
                    createdProcedure.PatientId,

                DoctorId=
                    createdProcedure.DoctorId,

                ProcedureName =
                    createdProcedure.ProcedureName,

                ProcedureDate =
                    createdProcedure.ProcedureDate,

                HospitalName =
                    createdProcedure.HospitalName,

                Reason =
                    createdProcedure.Reason,

                Outcome =
                    createdProcedure.Outcome,

                Notes =
                    createdProcedure.Notes
            };
        }
    }
}
