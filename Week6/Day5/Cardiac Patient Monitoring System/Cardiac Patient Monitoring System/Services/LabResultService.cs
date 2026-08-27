using Cardiac_Patient_Monitoring_System.DTO_S.LabResultDto_s;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Services.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class LabResultService : ILabResultService
    {
        private readonly ILabResultRepository _labResultRepository;
        private readonly IPatientRepository _patientRepository;

        public LabResultService(
            ILabResultRepository labResultRepository,
            IPatientRepository patientRepository)
        {
            _labResultRepository = labResultRepository;
            _patientRepository = patientRepository;
        }


        // Retrieves lab results for a specific patient within a date range.
        public async Task<IEnumerable<LabResultResponseDto>>
            GetByPatientAndDateRangeAsync(
                int patientId,
                DateTime startDate,
                DateTime endDate)
        {
            var labResults =
                await _labResultRepository
                    .GetForCardiacEventAnalysisAsync(
                        patientId,
                        startDate,
                        endDate);

            return labResults.Select(x =>
                new LabResultResponseDto
                {
                    LabResultId = x.LabResultId,
                    PatientId = x.PatientId,
                    TestName = x.TestName,
                    Result = x.Result,
                    Unit = x.Unit,
                    ReferenceRange = x.ReferenceRange,
                    TestDate = x.TestDate,
                    LaboratoryName = x.LaboratoryName,
                    Notes = x.Notes
                });
        }


        // Retrieves lab results for a specific patient within a date range.
        public async Task<LabResultResponseDto?>
    CreateAsync(
        int userId,
        CreateLabResultDto dto)
        {
            var patientId =
                await _labResultRepository
                    .GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
                return null;

            var labResult = new LabResult
            {
                PatientId = patientId.Value,

                TestName = dto.TestName,

                Result = dto.Result,

                Unit = dto.Unit,

                ReferenceRange = dto.ReferenceRange,

                TestDate = dto.TestDate,

                LaboratoryName = dto.LaboratoryName,

                Notes = dto.Notes,

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow
            };

            var createdLabResult =
                await _labResultRepository
                    .AddAsync(labResult);

            return new LabResultResponseDto
            {
                LabResultId =
                    createdLabResult.LabResultId,

                PatientId =
                    createdLabResult.PatientId,

                TestName =
                    createdLabResult.TestName,

                Result =
                    createdLabResult.Result,

                Unit =
                    createdLabResult.Unit,

                ReferenceRange =
                    createdLabResult.ReferenceRange,

                TestDate =
                    createdLabResult.TestDate,

                LaboratoryName =
                    createdLabResult.LaboratoryName,

                Notes =
                    createdLabResult.Notes
            };
        
    }
    }
}