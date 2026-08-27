using Cardiac_Patient_Monitoring_System.DTO_S.HospitalizationDto_s;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Services.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class HospitalizationService
        : IHospitalizationService
    {
        private readonly IHospitalizationRepository _repository;

        public HospitalizationService(
            IHospitalizationRepository repository)
        {
            _repository = repository;
        }


        // Retrieves hospitalizations that overlap with the specified date range.
        public async Task<IEnumerable<HospitalizationResponseDto>>
            GetOverlappingPeriodAsync(
                int patientId,
                DateTime startDate,
                DateTime eventDate)
        {
            var results =
                await _repository
                    .GetOverlappingPeriodAsync(
                        patientId,
                        startDate,
                        eventDate);

            return results.Select(x =>
                new HospitalizationResponseDto
                {
                    HospitalizationId =
                        x.HospitalizationId,

                    PatientId =
                        x.PatientId,

                    HospitalName =
                        x.HospitalName,

                    AdmissionDate =
                        x.AdmissionDate,

                    DischargeDate =
                        x.DischargeDate,

                    Reason =
                        x.Reason,

                    Diagnosis =
                        x.Diagnosis,

                    Notes =
                        x.Notes
                });
        }


        // Creates a new hospitalization record for the authenticated patient.
        public async Task<HospitalizationResponseDto?> CreateAsync(
int userId,
CreateHospitalizationDto dto)
        {
            var patientId =
                await _repository
                    .GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
            {
                return null;
            }

            var hospitalization = new Hospitalization
            {
                PatientId =
                    patientId.Value,

                HospitalName =
                    dto.HospitalName,

                AdmissionDate =
                    dto.AdmissionDate,

                DischargeDate =
                    dto.DischargeDate,

                Reason =
                    dto.Reason,

                Diagnosis =
                    dto.Diagnosis,

                Notes =
                    dto.Notes,

                CreatedAt =
                    DateTime.UtcNow,

                UpdatedAt =
                    DateTime.UtcNow
            };

            var created =
                await _repository
                    .AddAsync(hospitalization);

            return new HospitalizationResponseDto
            {
                HospitalizationId =
                    created.HospitalizationId,

                PatientId =
                    created.PatientId,

                HospitalName =
                    created.HospitalName,

                AdmissionDate =
                    created.AdmissionDate,

                DischargeDate =
                    created.DischargeDate,

                Reason =
                    created.Reason,

                Diagnosis =
                    created.Diagnosis,

                Notes =
                    created.Notes
            };
        }
    }
    }
