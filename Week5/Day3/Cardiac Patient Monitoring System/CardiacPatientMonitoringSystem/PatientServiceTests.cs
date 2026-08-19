//
// This test class contains unit tests for the PatientService.
//
// It uses xUnit for writing and running tests and Moq for mocking
// the IPatientRepository dependency.
//
// The tests cover patient profile management, patient collection
// operations, health summary generation, and health status evaluation.
//
// ======================= Profile Tests ============================
//
// 1. GetMyProfileAsync_ReturnsPatient_WhenPatientExists
//    - Verifies that a patient's profile is returned when a valid
//      user ID is linked to an existing patient.
//    - Verifies that the returned DTO contains the correct patient data.
//    - Verifies that the repository is called once.
//
// 2. GetMyProfileAsync_ReturnsNull_WhenPatientDoesNotExist
//    - Verifies that null is returned when no patient is linked
//      to the provided user ID.
//    - Ensures that the repository is queried correctly.
//
// 3. UpdateMyProfileAsync_PatientExists_UpdatesPatient
//    - Verifies successful update of the patient's editable profile data.
//    - Checks updated name, date of birth, gender, and phone number.
//    - Verifies that UpdateAsync and SaveChangesAsync are called.
//
// 4. UpdateMyProfileAsync_ReturnsNull_WhenPatientDoesNotExist
//    - Verifies that updating a non-existing patient returns null.
//    - Ensures that the repository update and save methods are not called.
//
// ======================= Patient Tests ============================
//
// 5. GetAllPatientsAsync_ReturnsAllPatients
//    - Verifies that all patients returned by the repository
//      are mapped correctly to PatientResponseDto objects.
//    - Checks the number and basic data of returned patients.
//
// 6. DeletePatientAsync_ReturnsTrue_WhenPatientExists
//    - Verifies successful deletion when the patient exists.
//    - Ensures that DeleteAsync and SaveChangesAsync are called once.
//
// 7. DeletePatientAsync_ReturnsFalse_WhenPatientDoesNotExist
//    - Verifies that deletion returns false when the patient
//      does not exist.
//    - Ensures that no delete or save operation is performed.
//
// ===================== Health Summary Tests ======================
//
// 8. GetMyHealthSummaryAsync_ReturnsSummary_WhenPatientExists
//    - Verifies that a complete health summary is generated
//      for an existing patient.
//    - Checks patient information.
//    - Checks the latest vital signs.
//    - Checks active medications.
//    - Checks recent diagnoses.
//    - Checks the upcoming scheduled appointment.
//
// ====================== Health Status Tests ======================
//
// 9. GetMyHealthStatusAsync_ReturnsNeedsAttention_WhenLatestVitalIsAbnormal
//    - Verifies that abnormal latest vital signs produce
//      "Needs Attention" status.
//    - Checks that the expected alerts are generated for:
//      high heart rate,
//      high systolic pressure,
//      high diastolic pressure,
//      low oxygen saturation,
//      and elevated temperature.
//
// 10. GetMyHealthStatusAsync_ReturnsNoData_WhenPatientHasNoVitalSigns
//     - Verifies that "No Data" is returned when the patient
//       has no vital-sign records.
//     - Ensures that no alerts or measurement date are returned.
//
// =========================== Coverage =============================
//
// The tests cover:
// - Successful profile retrieval.
// - Missing patient scenarios.
// - Profile updates.
// - Retrieving all patients.
// - Patient deletion success and failure.
// - Health summary business logic.
// - Active medication selection.
// - Recent diagnosis selection.
// - Upcoming appointment selection.
// - Health status and vital-sign alert logic.
// - Repository interaction using Moq.
//
using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.Summary;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Services;
using Moq;
using Xunit;

namespace CardiacPatientMonitoringSystem.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository>
            _repositoryMock;

        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _repositoryMock =
                new Mock<IPatientRepository>();

            _service =
                new PatientService(
                    _repositoryMock.Object);
        }

        // =========================================================
        // GetMyProfileAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task GetMyProfileAsync_ReturnsPatient_WhenPatientExists()
        {
            // Arrange

            var patient =
                CreatePatient();

            _repositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(1))
                .ReturnsAsync(patient);

            // Act

            var result =
                await _service.GetMyProfileAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                patient.PatientId,
                result.PatientId);

            Assert.Equal(
                patient.FirstName,
                result.FirstName);

            Assert.Equal(
                patient.LastName,
                result.LastName);

            Assert.Equal(
                patient.PatientGender.ToString(),
                result.PatientGender);

            Assert.Equal(
                patient.NationalId,
                result.NationalId);

            _repositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(1),
                Times.Once);
        }

        // =========================================================
        // GetMyProfileAsync - NOT FOUND
        // =========================================================

        [Fact]
        public async Task GetMyProfileAsync_ReturnsNull_WhenPatientDoesNotExist()
        {
            // Arrange

            _repositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(999))
                .ReturnsAsync(
                    (Patient?)null);

            // Act

            var result =
                await _service.GetMyProfileAsync(999);

            // Assert

            Assert.Null(result);

            _repositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(999),
                Times.Once);
        }

        // =========================================================
        // UpdateMyProfileAsync - SUCCESS
        // =========================================================

        [Fact]
    
public async Task UpdateMyProfileAsync_PatientExists_UpdatesPatient()
        {
            // Arrange
            var patient = CreatePatient();

            var dto = new UpdatePatientDto
            {
                FirstName = "Updated",
                LastName = "Patient",
                DateOfBirth = new DateTime(1997, 5, 15),
                PatientGender = Patient.Gender.Female,
                PrimaryPhone = "0591111111"
            };

            _repositoryMock
                .Setup(x => x.GetByUserIdAsync(1))
                .ReturnsAsync(patient);

            _repositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _service.UpdateMyProfileAsync(1, dto);

            // Assert
            Assert.NotNull(result);

            Assert.Equal("Updated", result.FirstName);
            Assert.Equal("Patient", result.LastName);
            Assert.Equal(
                new DateTime(1997, 5, 15),
                result.DateOfBirth);
            Assert.Equal(
                "Female",
                result.PatientGender);
            Assert.Equal(
                "0591111111",
                result.PrimaryPhone);

            _repositoryMock.Verify(
                x => x.UpdateAsync(
                    It.Is<Patient>(p =>
                        p.FirstName == "Updated" &&
                        p.LastName == "Patient" &&
                        p.DateOfBirth ==
                            new DateTime(1997, 5, 15) &&
                        p.PatientGender ==
                            Patient.Gender.Female &&
                        p.PrimaryPhone ==
                            "0591111111")),
                Times.Once);

            _repositoryMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
        
        // =========================================================
        // UpdateMyProfileAsync - NOT FOUND
        // =========================================================

        [Fact]
        public async Task UpdateMyProfileAsync_ReturnsNull_WhenPatientDoesNotExist()
        {
            // Arrange

            var dto =
                new UpdatePatientDto
                {
                    FirstName = "Updated",
                    LastName = "Patient",
                    DateOfBirth =
                        new DateTime(1997, 5, 15),
                    PatientGender =
                        Patient.Gender.Male,
                    PrimaryPhone =
                        "0591111111"
                };

            _repositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(999))
                .ReturnsAsync(
                    (Patient?)null);

            // Act

            var result =
                await _service.UpdateMyProfileAsync(
                    999,
                    dto);

            // Assert

            Assert.Null(result);

            _repositoryMock.Verify(
                x =>
                    x.UpdateAsync(
                        It.IsAny<Patient>()),
                Times.Never);

            _repositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }

        // =========================================================
        // GetAllPatientsAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsAllPatients()
        {
            // Arrange

            var patients =
                new List<Patient>
                {
                    CreatePatient(1, 1, "Ahmed", "Ali"),
                    CreatePatient(2, 2, "Sara", "Ahmad")
                };

            _repositoryMock
                .Setup(x =>
                    x.GetAllAsync())
                .ReturnsAsync(patients);

            // Act

            var result =
                await _service.GetAllPatientsAsync();

            // Assert

            Assert.NotNull(result);

            var resultList =
                result.ToList();

            Assert.Equal(
                2,
                resultList.Count);

            Assert.Equal(
                "Ahmed",
                resultList[0].FirstName);

            Assert.Equal(
                "Sara",
                resultList[1].FirstName);

            _repositoryMock.Verify(
                x =>
                    x.GetAllAsync(),
                Times.Once);
        }

        // =========================================================
        // DeletePatientAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task DeletePatientAsync_ReturnsTrue_WhenPatientExists()
        {
            // Arrange

            var patient =
                CreatePatient();

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _repositoryMock
                .Setup(x =>
                    x.DeleteAsync(patient))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(x =>
                    x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act

            var result =
                await _service.DeletePatientAsync(1);

            // Assert

            Assert.True(result);

            _repositoryMock.Verify(
                x =>
                    x.DeleteAsync(patient),
                Times.Once);

            _repositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Once);
        }

        // =========================================================
        // DeletePatientAsync - NOT FOUND
        // =========================================================

        [Fact]
        public async Task DeletePatientAsync_ReturnsFalse_WhenPatientDoesNotExist()
        {
            // Arrange

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(999))
                .ReturnsAsync(
                    (Patient?)null);

            // Act

            var result =
                await _service.DeletePatientAsync(999);

            // Assert

            Assert.False(result);

            _repositoryMock.Verify(
                x =>
                    x.DeleteAsync(
                        It.IsAny<Patient>()),
                Times.Never);

            _repositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }

        // =========================================================
        // GetMyHealthSummaryAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task GetMyHealthSummaryAsync_ReturnsSummary_WhenPatientExists()
        {
            // Arrange

            var patient =
                CreatePatientWithHealthData();

            _repositoryMock
                .Setup(x =>
                    x.GetWithHealthDataByUserIdAsync(1))
                .ReturnsAsync(patient);

            // Act

            var result =
                await _service.GetMyHealthSummaryAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.NotNull(
                result.Patient);

            Assert.Equal(
                patient.PatientId,
                result.Patient.PatientId);

            Assert.Equal(
                "Ahmed Ali",
                result.Patient.FullName);

            Assert.NotNull(
                result.LatestVitalSigns);

            Assert.Equal(
                80,
                result.LatestVitalSigns.HeartRate);

            Assert.NotNull(
                result.ActiveMedications);

            Assert.Single(
                result.ActiveMedications);

            Assert.NotNull(
                result.RecentDiagnoses);

            Assert.Single(
                result.RecentDiagnoses);

            Assert.NotNull(
                result.UpcomingAppointment);

            Assert.Equal(
                1,
                result.UpcomingAppointment.AppointmentId);

            _repositoryMock.Verify(
                x =>
                    x.GetWithHealthDataByUserIdAsync(1),
                Times.Once);
        }

        // =========================================================
        // GetMyHealthStatusAsync - NEEDS ATTENTION
        // =========================================================

        [Fact]
        public async Task GetMyHealthStatusAsync_ReturnsNeedsAttention_WhenLatestVitalIsAbnormal()
        {
            // Arrange

            var patient =
                CreatePatient();

            patient.VitalSigns.Add(
                new VitalSign
                {
                    VitalSignId = 10,
                    PatientId = patient.PatientId,

                    HeartRate = 120,
                    SystolicPressure = 150,
                    DiastolicPressure = 95,
                    OxygenSaturation = 88,
                    Temperature = 38.5m,

                    MeasuredAt =
                        DateTime.UtcNow,

                    CreatedAt =
                        DateTime.UtcNow
                });

            _repositoryMock
                .Setup(x =>
                    x.GetWithHealthDataByUserIdAsync(1))
                .ReturnsAsync(patient);

            // Act

            var result =
                await _service.GetMyHealthStatusAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                "Needs Attention",
                result.Status);

            Assert.Contains(
                "High heart rate.",
                result.Alerts);

            Assert.Contains(
                "High systolic pressure.",
                result.Alerts);

            Assert.Contains(
                "High diastolic pressure.",
                result.Alerts);

            Assert.Contains(
                "Low oxygen saturation.",
                result.Alerts);

            Assert.Contains(
                "Elevated temperature.",
                result.Alerts);

            Assert.NotNull(
                result.LatestMeasuredAt);
        }

        // =========================================================
        // GetMyHealthStatusAsync - NO DATA
        // =========================================================

        [Fact]
        public async Task GetMyHealthStatusAsync_ReturnsNoData_WhenPatientHasNoVitalSigns()
        {
            // Arrange

            var patient =
                CreatePatient();

            _repositoryMock
                .Setup(x =>
                    x.GetWithHealthDataByUserIdAsync(1))
                .ReturnsAsync(patient);

            // Act

            var result =
                await _service.GetMyHealthStatusAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                "No Data",
                result.Status);

            Assert.Empty(
                result.Alerts);

            Assert.Null(
                result.LatestMeasuredAt);
        }

        // =========================================================
        // Helper: Basic Patient
        // =========================================================

        private static Patient CreatePatient(
            int patientId = 1,
            int userId = 1,
            string firstName = "Ahmed",
            string lastName = "Ali")
        {
            return new Patient
            {
                PatientId = patientId,

                UserId = userId,

                FirstName =
                    firstName,

                LastName =
                    lastName,

                DateOfBirth =
                    new DateTime(
                        1995,
                        1,
                        1),

                PatientGender =
                    Patient.Gender.Male,

                PrimaryPhone =
                    "0599999999",

                NationalId =
                    $"TEST{patientId}123",

                CreatedAt =
                    DateTime.UtcNow.AddDays(-10),

                UpdatedAt =
                    DateTime.UtcNow.AddDays(-5)
            };
        }

        // =========================================================
        // Helper: Patient With Health Data
        // =========================================================

        private static Patient
            CreatePatientWithHealthData()
        {
            var patient =
                CreatePatient();

            patient.VitalSigns.Add(
                new VitalSign
                {
                    VitalSignId = 1,
                    PatientId =
                        patient.PatientId,

                    HeartRate = 80,

                    SystolicPressure = 120,

                    DiastolicPressure = 80,

                    OxygenSaturation = 98,

                    Temperature = 36.8m,

                    MeasuredAt =
                        DateTime.UtcNow,

                    CreatedAt =
                        DateTime.UtcNow
                });

            patient.Medications.Add(
                new Medication
                {
                    MedicationId = 1,

                    PatientId =
                        patient.PatientId,

                    Name = "Aspirin",

                    Dosage = "100mg",

                    Frequency =
                        "Once Daily",

                    StartDate =
                        DateTime.Today.AddDays(-5),

                    EndDate = null,

                    CreatedAt =
                        DateTime.UtcNow,

                    UpdatedAt =
                        DateTime.UtcNow
                });

            patient.Diagnoses.Add(
                new Diagnosis
                {
                    DiagnosisId = 1,

                    PatientId =
                        patient.PatientId,

                    DiagnosisName =
                        "Hypertension",

                    DiagnosedAt =
                        DateTime.UtcNow.AddDays(-2),

                    DiagnosedByName =
                        "Dr. Ahmad",

                    DiagnosedBySpecialization =
                        "Cardiology",

                    Notes =
                        "Regular monitoring.",

                    CreatedAt =
                        DateTime.UtcNow,

                    UpdatedAt =
                        DateTime.UtcNow
                });

            patient.Appointments.Add(
                new Appointment
                {
                    AppointmentId = 1,

                    PatientId =
                        patient.PatientId,

                    AppointmentDate =
                        DateTime.UtcNow.AddDays(3),

                    Reason =
                        "Follow-up",

                    Status =
                        Appointment.AppointmentStatus.Scheduled,

                    Location =
                        "Cardiac Clinic",

                    Notes =
                        "Regular follow-up.",

                    CreatedAt =
                        DateTime.UtcNow,

                    UpdatedAt =
                        DateTime.UtcNow
                });

            return patient;
        }
    }
}