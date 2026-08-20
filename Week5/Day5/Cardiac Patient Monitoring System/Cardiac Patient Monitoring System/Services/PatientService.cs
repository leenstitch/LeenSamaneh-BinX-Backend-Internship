using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.Summary;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<PatientResponseDto?> GetMyProfileAsync(int userId)
        {
            var patient =
                await _patientRepository.GetByUserIdAsync(userId);

            if (patient == null)
                return null;

            return new PatientResponseDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                PatientGender = patient.PatientGender.ToString(),
                PrimaryPhone = patient.PrimaryPhone,
                NationalId = patient.NationalId,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };
        }

        public async Task<PatientResponseDto?> UpdateMyProfileAsync(
     int userId,
     UpdatePatientDto dto)
        {
         
            var patient =
                await _patientRepository.GetByUserIdAsync(userId);

            if (patient == null)
                return null;

            if (dto.FirstName != null)
                patient.FirstName = dto.FirstName;

            if (dto.LastName != null)
                patient.LastName = dto.LastName;

            if (dto.DateOfBirth.HasValue)
                patient.DateOfBirth = dto.DateOfBirth.Value;

            if (dto.PatientGender.HasValue)
                patient.PatientGender = dto.PatientGender.Value;

            if (dto.PrimaryPhone != null)
                patient.PrimaryPhone = dto.PrimaryPhone;

            patient.UpdatedAt = DateTime.UtcNow;

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.SaveChangesAsync();

            return new PatientResponseDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                PatientGender = patient.PatientGender.ToString(),
                PrimaryPhone = patient.PrimaryPhone,
                NationalId = patient.NationalId,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };
        }

        public async Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync()
        {
            var patients =
                await _patientRepository.GetAllAsync();

            return patients.Select(patient => new PatientResponseDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                PatientGender = patient.PatientGender.ToString(),
                PrimaryPhone = patient.PrimaryPhone,
                NationalId = patient.NationalId,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            });
        }

        public async Task<bool> DeletePatientAsync(int patientId)
        {
            var patient =
                await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
                return false;

            await _patientRepository.DeleteAsync(patient);
            await _patientRepository.SaveChangesAsync();

            return true;
        }
        public async Task<PatientHealthSummaryDto?>
    GetMyHealthSummaryAsync(int userId)
        {
            var patient =
                await _patientRepository
                    .GetWithHealthDataByUserIdAsync(userId);

            if (patient == null)
                return null;

            return BuildHealthSummary(patient);
        }
        public async Task<PatientHealthSummaryDto?>
    GetHealthSummaryAsync(int patientId)
        {
            var patient =
                await _patientRepository
                    .GetWithHealthDataByIdAsync(patientId);

            if (patient == null)
                return null;

            return BuildHealthSummary(patient);
        }


        public async Task<PatientHealthStatusDto?>
    GetMyHealthStatusAsync(int userId)
        {
            var patient =
                await _patientRepository
                    .GetWithHealthDataByUserIdAsync(userId);

            if (patient == null)
                return null;

            return BuildHealthStatus(patient);
        }

        public async Task<PatientHealthStatusDto?>
            GetHealthStatusAsync(int patientId)
        {
            var patient =
                await _patientRepository
                    .GetWithHealthDataByIdAsync(patientId);

            if (patient == null)
                return null;

            return BuildHealthStatus(patient);
        }


        private static PatientHealthSummaryDto
    BuildHealthSummary(Patient patient)
        {
            var today = DateTime.Today;

            var age =
                today.Year - patient.DateOfBirth.Year;

            if (patient.DateOfBirth.Date >
                today.AddYears(-age))
            {
                age--;
            }

            var latestVitalSign =
                patient.VitalSigns
                    .OrderByDescending(v => v.MeasuredAt)
                    .FirstOrDefault();

            var activeMedications =
                patient.Medications
                    .Where(m =>
                        m.StartDate.Date <= today &&
                        (!m.EndDate.HasValue ||
                         m.EndDate.Value.Date >= today))
                    .OrderByDescending(m => m.StartDate)
                    .Select(m => new MedicationSummaryDto
                    {
                        MedicationId = m.MedicationId,
                        Name = m.Name,
                        Dosage = m.Dosage,
                        Frequency = m.Frequency,
                        StartDate = m.StartDate,
                        EndDate = m.EndDate
                    })
                    .ToList();

            var recentDiagnoses =
                patient.Diagnoses
                    .OrderByDescending(d => d.DiagnosedAt)
                    .Take(5)
                    .Select(d => new DiagnosisSummaryDto
                    {
                        DiagnosisId = d.DiagnosisId,
                        DiagnosisName = d.DiagnosisName,
                        DiagnosedAt = d.DiagnosedAt,
                        DiagnosedByName = d.DiagnosedByName,
                        DiagnosedBySpecialization =
                            d.DiagnosedBySpecialization,
                        Notes = d.Notes
                    })
                    .ToList();

            var upcomingAppointment =
                patient.Appointments
                    .Where(a =>
                        a.AppointmentDate >= DateTime.UtcNow &&
                        a.Status ==
                        Appointment.AppointmentStatus.Scheduled)
                    .OrderBy(a => a.AppointmentDate)
                    .FirstOrDefault();

            return new PatientHealthSummaryDto
            {
                Patient = new PatientSummaryDto
                {
                    PatientId = patient.PatientId,
                    FullName =
                        $"{patient.FirstName} {patient.LastName}",
                    Age = age,
                    Gender =
                        patient.PatientGender.ToString(),
                    PrimaryPhone =
                        patient.PrimaryPhone
                },

                LatestVitalSigns =
                    latestVitalSign == null
                        ? null
                        : new VitalSignSummaryDto
                        {
                            HeartRate =
                                latestVitalSign.HeartRate,
                            SystolicPressure =
                                latestVitalSign.SystolicPressure,
                            DiastolicPressure =
                                latestVitalSign.DiastolicPressure,
                            OxygenSaturation =
                                latestVitalSign.OxygenSaturation,
                            Temperature =
                                latestVitalSign.Temperature,
                            MeasuredAt =
                                latestVitalSign.MeasuredAt,
                            Notes =
                                latestVitalSign.Notes
                        },

                ActiveMedications =
                    activeMedications,

                RecentDiagnoses =
                    recentDiagnoses,

                UpcomingAppointment =
                    upcomingAppointment == null
                        ? null
                        : new AppointmentSummaryDto
                        {
                            AppointmentId =
                                upcomingAppointment.AppointmentId,
                            AppointmentDate =
                                upcomingAppointment.AppointmentDate,
                            Reason =
                                upcomingAppointment.Reason,
                            Status =
                                upcomingAppointment.Status.ToString(),
                            Location =
                                upcomingAppointment.Location,
                            Notes =
                                upcomingAppointment.Notes
                        }
            };
        }
        private static PatientHealthStatusDto
    BuildHealthStatus(Patient patient)
        {
            var latestVitalSign =
    patient.VitalSigns
        .OrderByDescending(v => v.VitalSignId)
        .FirstOrDefault();

            if (latestVitalSign == null)
            {
                return new PatientHealthStatusDto
                {
                    Status = "No Data"
                };
            }
            Console.WriteLine(
    $"Latest Vital => HR: {latestVitalSign.HeartRate}, " +
    $"Systolic: {latestVitalSign.SystolicPressure}, " +
    $"Diastolic: {latestVitalSign.DiastolicPressure}, " +
    $"O2: {latestVitalSign.OxygenSaturation}, " +
    $"Temp: {latestVitalSign.Temperature}, " +
    $"MeasuredAt: {latestVitalSign.MeasuredAt}");
            var alerts = new List<string>();

            if (latestVitalSign.HeartRate > 100)
                alerts.Add("High heart rate.");

            if (latestVitalSign.HeartRate < 60)
                alerts.Add("Low heart rate.");

            if (latestVitalSign.SystolicPressure >= 140)
                alerts.Add("High systolic pressure.");

            if (latestVitalSign.DiastolicPressure >= 90)
                alerts.Add("High diastolic pressure.");

            if (latestVitalSign.OxygenSaturation < 92)
                alerts.Add("Low oxygen saturation.");

            if (latestVitalSign.Temperature >= 38)
                alerts.Add("Elevated temperature.");

            return new PatientHealthStatusDto
            {
                Status = alerts.Count == 0
                    ? "Stable"
                    : "Needs Attention",

                Alerts = alerts,

                LatestMeasuredAt =
                    latestVitalSign.MeasuredAt
            };
        }
    }
}