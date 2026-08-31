// This service handles the business logic for appointments.
// It provides operations for retrieving, creating, filtering,
// updating, and changing appointment status.

using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s.AppointmentWithMedicalIntakeDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Repositories.Repository;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAllergyRepository _allergyRepository;
        private readonly IFamilyMedicalHistoryRepository _familyHistoryRepository;
        private readonly IMedicationRepository _medicationRepository;
        private readonly IDiagnosisRepository _diagnosisRepository;
        private readonly IEmergencyMedicalInformationRepository _emergencyRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ApplicationDbContext _context;
        public AppointmentService(
            IAppointmentRepository repository,
            IAppointmentRepository appointmentRepository,
            IAllergyRepository allergyRepository,
            IFamilyMedicalHistoryRepository familyHistoryRepository,
            IMedicationRepository medicationRepository,
            IDiagnosisRepository diagnosisRepository,
            IEmergencyMedicalInformationRepository emergencyRepository,
            IPatientRepository patientRepository,
            ApplicationDbContext context)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _allergyRepository = allergyRepository;
            _familyHistoryRepository = familyHistoryRepository;
            _medicationRepository = medicationRepository;
            _diagnosisRepository = diagnosisRepository;
            _emergencyRepository = emergencyRepository;
            _patientRepository = patientRepository;
            _context = context;
        }

        // Returns a single appointment by its ID.
        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment =
                await _repository.GetByIdAsync(id);

            if (appointment == null)
                return null;

            return new AppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                RecordedByDoctorName = appointment.RecordedByDoctorName,
                AppointmentDate = appointment.AppointmentDate,
                Reason = appointment.Reason,
                Status = appointment.Status.ToString(),
                Notes = appointment.Notes,
                Location = appointment.Location,
                CreatedAt = appointment.CreatedAt,
                UpdatedAt = appointment.UpdatedAt
            };
        }

        // Returns appointments belonging to the authenticated patient.
        public async Task<IEnumerable<AppointmentDto>> GetMyAppointmentsAsync(int userId)
        {
            var patientId =
                await _repository.GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
                return Enumerable.Empty<AppointmentDto>();

            var appointments =
                await _repository.GetByPatientIdAsync(
                    patientId.Value);

            return appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                RecordedByDoctorName = a.RecordedByDoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                CreatedAt = a.CreatedAt,
                Location = a.Location,
                UpdatedAt = a.UpdatedAt
            });
        }

        // Returns all appointments.
        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _repository.GetAllAsync();

            return appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                RecordedByDoctorName = a.RecordedByDoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                Location = a.Location,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            });
        }

        // Creates a new appointment for the patient linked to the user.
        // New appointments are automatically created with Scheduled status.
        public async Task<Appointment?> CreateAsync(
            int userId,
            CreateAppointmentDto dto)
        {
            var patientId =
                await _repository.GetPatientIdByUserIdAsync(
                    userId);

            if (!patientId.HasValue)
                return null;

            var appointment = new Appointment
            {
                PatientId = patientId.Value,

                AppointmentDate = dto.AppointmentDate,

                Reason = dto.Reason,

                Location = dto.Location,

                Notes = dto.Notes,

                RecordedByDoctorName = dto.RecordedByDoctorName,

                Status = Appointment.AppointmentStatus.Scheduled,

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow
            };

            return await _repository.AddAsync(appointment);
        }

        // Returns all appointments belonging to a specific patient.
        public async Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId)
        {
            var appointments =
                await _repository.GetByPatientIdAsync(patientId);

            return appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                RecordedByDoctorName = a.RecordedByDoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                CreatedAt = a.CreatedAt,
                Location = a.Location,
                UpdatedAt = a.UpdatedAt
            });
        }

        // Filters appointments belonging to the authenticated patient.
        public async Task<IEnumerable<AppointmentDto>>
            FilterMyAppointmentsAsync(
                int userId,
                AppointmentFilterDto filter)
        {
            var patientId =
                await _repository.GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
                return Enumerable.Empty<AppointmentDto>();

            var appointments =
                await _repository.FilterAsync(
                    patientId.Value,
                    filter.Date,
                    filter.Year,
                    filter.Month,
                    filter.Status);

            return appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                RecordedByDoctorName = a.RecordedByDoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                CreatedAt = a.CreatedAt,
                Location = a.Location,
                UpdatedAt = a.UpdatedAt
            });
        }

        // Filters appointments across all patients.
        public async Task<IEnumerable<AppointmentDto>>
            FilterAllAsync(
                AppointmentFilterDto filter)
        {
            var appointments =
                await _repository.FilterAsync(
                    null,
                    filter.Date,
                    filter.Year,
                    filter.Month,
                    filter.Status);

            return appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                RecordedByDoctorName = a.RecordedByDoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                CreatedAt = a.CreatedAt,
                Location = a.Location,
                UpdatedAt = a.UpdatedAt
            });
        }

        // Updates appointment information for the authenticated patient.
        public async Task<bool> UpdateAsync(
            int id,
            int userId,
            UpdateAppointmentDto dto)
        {
            var appointment =
                await _repository.GetByIdAsync(id);

            if (appointment == null)
                return false;

            var patientId =
                await _repository.GetPatientIdByUserIdAsync(
                    userId);

            // Ensures that the appointment belongs to
            // the authenticated patient.
            if (!patientId.HasValue ||
                appointment.PatientId != patientId.Value)
            {
                return false;
            }

            // Only Scheduled appointments can be edited.
            if (appointment.Status !=
                Appointment.AppointmentStatus.Scheduled)
            {
                return false;
            }

            if (dto.AppointmentDate.HasValue)
            {
                appointment.AppointmentDate =
                    dto.AppointmentDate.Value;
            }

            if (dto.Reason != null)
            {
                appointment.Reason = dto.Reason;
            }

            if (dto.Location != null)
            {
                appointment.Location = dto.Location;
            }

            if (dto.Notes != null)
            {
                appointment.Notes = dto.Notes;
            }

            appointment.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(appointment);

            return true;
        }

        // Updates only the status of an appointment.
        public async Task<bool> UpdateStatusAsync(
            int id,
            int userId,
            UpdateAppointmentStatusDto dto)
        {
            var appointment =
                await _repository.GetByIdAsync(id);

            if (appointment == null)
                return false;

            var patientId =
                await _repository.GetPatientIdByUserIdAsync(
                    userId);

            // Ensures that the appointment belongs to
            // the authenticated patient.
            if (!patientId.HasValue ||
                appointment.PatientId != patientId.Value)
            {
                return false;
            }

            // Only Scheduled appointments can change status.
            if (appointment.Status !=
                Appointment.AppointmentStatus.Scheduled)
            {
                return false;
            }

            // Only Completed or Cancelled statuses are allowed.
            if (dto.Status !=
                    Appointment.AppointmentStatus.Completed &&
                dto.Status !=
                    Appointment.AppointmentStatus.Cancelled)
            {
                return false;
            }

            appointment.Status =
                (Appointment.AppointmentStatus)dto.Status;

            appointment.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(appointment);

            return true;
        }
        public async Task<AppointmentWithMedicalIntakeResponseDto>
      CreateWithMedicalIntakeAsync(
          int userId,
          CreateAppointmentWithMedicalIntakeDto dto)
        {
            // 1. Get patient using UserId from token
            var patient =
                await _patientRepository
                    .GetByUserIdAsync(userId);

            if (patient == null)
            {
                throw new InvalidOperationException(
                    "Patient profile was not found.");
            }

            var patientId = patient.PatientId;

            // 2. Start transaction
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // 3. Get existing medical information

                var allergies =
                    await _allergyRepository
                        .GetByPatientIdAsync(patientId);

                var familyHistory =
                    await _familyHistoryRepository
                        .GetByPatientIdAsync(patientId);

                var medications =
                    await _medicationRepository
                        .GetByPatientIdAsync(patientId);

                var diagnoses =
                    await _diagnosisRepository
                        .GetByPatientIdAsync(patientId);

                var emergencyMedicalInformation =
                    await _emergencyRepository
                        .GetByPatientIdAsync(patientId);


                // 4. Create new allergies

                var newAllergies = dto.NewAllergies
                    .Select(a => new Allergy
                    {
                        PatientId = patientId,

                        Name = a.Name,
                        Reaction = a.Reaction,
                        Severity = a.Severity,
                        DiagnosedAt = a.DiagnosedAt,
                        Notes = a.Notes,

                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    })
                    .ToList();

                if (newAllergies.Any())
                {
                    await _allergyRepository
                        .CreateRangeAsync(newAllergies);

                    allergies.AddRange(newAllergies);
                }


                // 5. Create new family medical history

                var newFamilyHistory = dto.NewFamilyHistory
                    .Select(f => new FamilyMedicalHistory
                    {
                        PatientId = patientId,

                        Relationship = f.Relationship,
                        Condition = f.Condition,
                        AgeAtDiagnosis = f.AgeAtDiagnosis,
                        Notes = f.Notes,

                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    })
                    .ToList();

                if (newFamilyHistory.Any())
                {
                    await _familyHistoryRepository
                        .CreateRangeAsync(newFamilyHistory);

                    familyHistory.AddRange(newFamilyHistory);
                }


                // 6. Create appointment

                var appointment = new Appointment
                {
                    PatientId = patientId,

                    AppointmentDate =
                        dto.AppointmentDate,

                    Reason =
                        dto.Reason,

                    Notes =
                        dto.Notes,

                    Status =
                        Appointment.AppointmentStatus.Scheduled,

                    CreatedAt =
                        DateTime.UtcNow,

                    UpdatedAt =
                        DateTime.UtcNow
                };

                _context.Appointments.Add(appointment);


                // 7. Save all changes

                await _context.SaveChangesAsync();


                // 8. Commit transaction

                await transaction.CommitAsync();


                // 9. Map entities to DTOs

                return new AppointmentWithMedicalIntakeResponseDto
                {
                    AppointmentId =
                        appointment.AppointmentId,

                    PatientId =
                        appointment.PatientId,

                    DoctorId =
                        appointment.DoctorId,

                    AppointmentDate =
                        appointment.AppointmentDate,

                    Reason =
                        appointment.Reason,

                    Notes =
                        appointment.Notes,


                    // Existing + newly created allergies

                    Allergies = allergies
                        .Select(a => new AllergyResponseDto
                        {
                            AllergyId =
                                a.AllergyId,

                            Name =
                                a.Name,

                            Reaction =
                                a.Reaction,

                            Severity =
                                a.Severity,

                            DiagnosedAt =
                                a.DiagnosedAt,

                            Notes =
                                a.Notes
                        })
                        .ToList(),


                    // Existing + newly created family history

                    FamilyHistory = familyHistory
                        .Select(f => new FamilyHistoryResponseDto
                        {
                            FamilyHistoryId =
                                f.FamilyHistoryId,

                            Relationship =
                                f.Relationship,

                            Condition =
                                f.Condition,

                            AgeAtDiagnosis =
                                f.AgeAtDiagnosis,

                            Notes =
                                f.Notes
                        })
                        .ToList(),


                    // Existing medications

                    Medications = medications
                        .Select(m => new MedicationResponseDto
                        {
                            MedicationId =
                                m.MedicationId,

                            PrescribedByDoctorName =
                                m.PrescribedByDoctorName,

                            PrescribedBySpecialization =
                                m.PrescribedBySpecialization,

                            Name =
                                m.Name,

                            Dosage =
                                m.Dosage,

                            Frequency =
                                m.Frequency,

                            StartDate =
                                m.StartDate,

                            EndDate =
                                m.EndDate
                        })
                        .ToList(),


                    // Existing diagnoses

                    Diagnoses = diagnoses
                        .Select(d => new DiagnosisResponseDto
                        {
                            DiagnosisId =
                                d.DiagnosisId,

                            DiagnosedByName =
                                d.DiagnosedByName,

                            DiagnosedBySpecialization =
                                d.DiagnosedBySpecialization,

                            DiagnosisName =
                                d.DiagnosisName,

                            DiagnosedAt =
                                d.DiagnosedAt,

                            Notes =
                                d.Notes,

                            ConditionStartDate =
                                d.ConditionStartDate
                        })
                        .ToList(),


                    // Emergency medical information

                    EmergencyMedicalInformation =
                        emergencyMedicalInformation == null
                            ? null
                            : new EmergencyMedicalInformationResponseDto
                            {
                                EmergencyMedicalInformationId =
                                    emergencyMedicalInformation
                                        .EmergencyMedicalInformationId,

                                BloodType =
                                    emergencyMedicalInformation
                                        .BloodType,

                                PreferredHospital =
                                    emergencyMedicalInformation
                                        .PreferredHospital,

                                SpecialInstructions =
                                    emergencyMedicalInformation
                                        .SpecialInstructions,

                                EmergencyNotes =
                                    emergencyMedicalInformation
                                        .EmergencyNotes
                            }
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}