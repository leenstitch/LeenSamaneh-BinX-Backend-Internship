using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.DTO_S.AllergyDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s.AppointmentWithMedicalIntakeDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.FamilyHistoryDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CardiacPatientMonitoringSystem.XunitMoq
{
    public class AppointmentServiceTests : IDisposable
    {
        // =========================================================
        // Repository Mocks
        // =========================================================

        private readonly Mock<IAppointmentRepository>
            _appointmentRepositoryMock;

        private readonly Mock<IAllergyRepository>
            _allergyRepositoryMock;

        private readonly Mock<IFamilyMedicalHistoryRepository>
            _familyHistoryRepositoryMock;

        private readonly Mock<IMedicationRepository>
            _medicationRepositoryMock;

        private readonly Mock<IDiagnosisRepository>
            _diagnosisRepositoryMock;

        private readonly Mock<IEmergencyMedicalInformationRepository>
            _emergencyRepositoryMock;

        private readonly Mock<IPatientRepository>
            _patientRepositoryMock;

        // =========================================================
        // SQLite In-Memory
        // =========================================================

        private readonly SqliteConnection _connection;

        private readonly ApplicationDbContext _context;

        // =========================================================
        // Service
        // =========================================================

        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            // Create SQLite in-memory connection

            _connection = new SqliteConnection(
                "DataSource=:memory:");

            _connection.Open();

            // Create DbContext options

            var options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(_connection)
                    .Options;

            // Create real DbContext

            _context =
                new ApplicationDbContext(options);

            // Create database schema

            _context.Database.EnsureCreated();

            // Create repository mocks

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _allergyRepositoryMock =
                new Mock<IAllergyRepository>();

            _familyHistoryRepositoryMock =
                new Mock<IFamilyMedicalHistoryRepository>();

            _medicationRepositoryMock =
                new Mock<IMedicationRepository>();

            _diagnosisRepositoryMock =
                new Mock<IDiagnosisRepository>();

            _emergencyRepositoryMock =
                new Mock<IEmergencyMedicalInformationRepository>();

            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            // Create service

            _service =
                new AppointmentService(
                    _appointmentRepositoryMock.Object,
                    _appointmentRepositoryMock.Object,
                    _allergyRepositoryMock.Object,
                    _familyHistoryRepositoryMock.Object,
                    _medicationRepositoryMock.Object,
                    _diagnosisRepositoryMock.Object,
                    _emergencyRepositoryMock.Object,
                    _patientRepositoryMock.Object,
                    _context);
        }

        // =========================================================
        // Dispose
        // =========================================================

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }

        // =========================================================
        // Test 1
        //
        // Create Appointment With Medical Intake - Success
        //
        // Verifies:
        // - Patient is found using UserId
        // - Existing medical information is retrieved
        // - New allergy is created
        // - New family history is created
        // - Appointment is created
        // - Appointment is saved to SQLite
        // - Correct response is returned
        // =========================================================

        [Fact]
        public async Task
            CreateWithMedicalIntakeAsync_ReturnsResponse_WhenPatientExists()
        {
            // =====================================================
            // Arrange
            // =====================================================

            // -----------------------------------------------------
            // Create ApplicationUser
            // -----------------------------------------------------

            var user = new ApplicationUser
            {
                Id = 10,
                UserName = "testuser",
                NormalizedUserName = "TESTUSER",
                Email = "test@example.com",
                NormalizedEmail = "TEST@EXAMPLE.COM",
                EmailConfirmed = true
            };

            _context.Users.Add(user);


            // -----------------------------------------------------
            // Create Patient
            // -----------------------------------------------------

            var patient = new Patient
            {
                PatientId = 5,
                UserId = 10
            };

            _context.Patients.Add(patient);


            // Save User + Patient to SQLite
            //
            // This is important because Appointment.PatientId
            // is a Foreign Key to Patient.
            //

            await _context.SaveChangesAsync();


            // -----------------------------------------------------
            // Mock Patient Repository
            // -----------------------------------------------------

            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(10))
                .ReturnsAsync(patient);


            // -----------------------------------------------------
            // Existing Allergies
            // -----------------------------------------------------

            var existingAllergies =
                new List<Allergy>
                {
                    new Allergy
                    {
                        AllergyId = 1,
                        PatientId = 5,

                        Name = "Penicillin",
                        Reaction = "Rash",
                        Severity = "Moderate",

                        DiagnosedAt =
                            new DateTime(
                                2026,
                                1,
                                1),

                        Notes = "Existing allergy",

                        CreatedAt =
                            DateTime.UtcNow,

                        UpdatedAt =
                            DateTime.UtcNow
                    }
                };


            _allergyRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(existingAllergies);


            // -----------------------------------------------------
            // Existing Family Medical History
            // -----------------------------------------------------

            var existingFamilyHistory =
                new List<FamilyMedicalHistory>();


            _familyHistoryRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(existingFamilyHistory);


            // -----------------------------------------------------
            // Existing Medications
            // -----------------------------------------------------

            var existingMedications =
                new List<Medication>();


            _medicationRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(existingMedications);


            // -----------------------------------------------------
            // Existing Diagnoses
            // -----------------------------------------------------

            var existingDiagnoses =
                new List<Diagnosis>();


            _diagnosisRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(existingDiagnoses);


            // -----------------------------------------------------
            // Emergency Medical Information
            // -----------------------------------------------------

            _emergencyRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(
                    (EmergencyMedicalInformation?)null);


            // -----------------------------------------------------
            // Create DTO
            // -----------------------------------------------------

            var dto =
                new CreateAppointmentWithMedicalIntakeDto
                {
                    AppointmentDate =
                        new DateTime(
                            2026,
                            9,
                            1,
                            10,
                            0,
                            0),

                    Reason =
                        "Cardiac follow-up",

                    Notes =
                        "Patient needs regular monitoring",

                    // ---------------------------------------------
                    // New Allergy
                    // ---------------------------------------------

                    NewAllergies =
                        new List<CreateAllergyDto>
                        {
                            new CreateAllergyDto
                            {
                                Name = "Aspirin",

                                Reaction =
                                    "Stomach pain",

                                Severity =
                                    "Mild",

                                DiagnosedAt =
                                    new DateTime(
                                        2026,
                                        8,
                                        1),

                                Notes =
                                    "New allergy"
                            }
                        },

                    // ---------------------------------------------
                    // New Family History
                    // ---------------------------------------------

                    NewFamilyHistory =
                        new List<CreateFamilyHistoryDto>
                        {
                            new CreateFamilyHistoryDto
                            {
                                Relationship =
                                    "Father",

                                Condition =
                                    "Hypertension",

                                AgeAtDiagnosis =
                                    50,

                                Notes =
                                    "Family history"
                            }
                        }
                };


            // -----------------------------------------------------
            // Mock CreateRangeAsync for Allergies
            // -----------------------------------------------------

            _allergyRepositoryMock
                .Setup(x =>
                    x.CreateRangeAsync(
                        It.IsAny<List<Allergy>>()))
                .Returns(Task.CompletedTask);


            // -----------------------------------------------------
            // Mock CreateRangeAsync for Family History
            // -----------------------------------------------------

            _familyHistoryRepositoryMock
                .Setup(x =>
                    x.CreateRangeAsync(
                        It.IsAny<List<FamilyMedicalHistory>>()))
                .Returns(Task.CompletedTask);


            // =====================================================
            // Act
            // =====================================================

            var result =
                await _service
                    .CreateWithMedicalIntakeAsync(
                        10,
                        dto);


            // =====================================================
            // Assert
            // =====================================================

            // -----------------------------------------------------
            // Response exists
            // -----------------------------------------------------

            Assert.NotNull(result);


            // -----------------------------------------------------
            // Appointment information
            // -----------------------------------------------------

            Assert.Equal(
                5,
                result.PatientId);

            Assert.Equal(
                dto.AppointmentDate,
                result.AppointmentDate);

            Assert.Equal(
                "Cardiac follow-up",
                result.Reason);

            Assert.Equal(
                "Patient needs regular monitoring",
                result.Notes);


            // -----------------------------------------------------
            // Existing + New Allergy
            // -----------------------------------------------------

            Assert.Equal(
                2,
                result.Allergies.Count);


            Assert.Contains(
                result.Allergies,
                a => a.Name == "Penicillin");


            Assert.Contains(
                result.Allergies,
                a => a.Name == "Aspirin");


            // -----------------------------------------------------
            // Existing + New Family History
            // -----------------------------------------------------

            Assert.Single(
                result.FamilyHistory);


            Assert.Equal(
                "Father",
                result.FamilyHistory[0].Relationship);


            Assert.Equal(
                "Hypertension",
                result.FamilyHistory[0].Condition);


            // -----------------------------------------------------
            // Emergency Information
            // -----------------------------------------------------

            Assert.Null(
                result.EmergencyMedicalInformation);


            // =====================================================
            // Verify Repository Calls
            // =====================================================

            _patientRepositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(10),
                Times.Once);


            _allergyRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(5),
                Times.Once);


            _familyHistoryRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(5),
                Times.Once);


            _medicationRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(5),
                Times.Once);


            _diagnosisRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(5),
                Times.Once);


            _emergencyRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(5),
                Times.Once);


            // -----------------------------------------------------
            // Verify new allergy
            // -----------------------------------------------------

            _allergyRepositoryMock.Verify(
                x =>
                    x.CreateRangeAsync(
                        It.Is<List<Allergy>>(
                            list =>
                                list.Count == 1
                                &&
                                list[0].PatientId == 5
                                &&
                                list[0].Name == "Aspirin")),
                Times.Once);


            // -----------------------------------------------------
            // Verify new family history
            // -----------------------------------------------------

            _familyHistoryRepositoryMock.Verify(
                x =>
                    x.CreateRangeAsync(
                        It.Is<List<FamilyMedicalHistory>>(
                            list =>
                                list.Count == 1
                                &&
                                list[0].PatientId == 5
                                &&
                                list[0].Relationship == "Father")),
                Times.Once);


            // =====================================================
            // Verify Appointment Was Actually Saved
            // =====================================================

            var savedAppointment =
                await _context.Appointments
                    .FirstOrDefaultAsync(
                        a =>
                            a.PatientId == 5
                            &&
                            a.Reason ==
                                "Cardiac follow-up");


            Assert.NotNull(
                savedAppointment);


            Assert.Equal(
                Appointment.AppointmentStatus.Scheduled,
                savedAppointment.Status);
        }

        // =========================================================
        // Test 2: Patient Not Found
        //
        // Verifies that an InvalidOperationException is thrown
        // when the patient profile does not exist.
        //
        // This is an error test.
        // Assert.ThrowsAsync is used to make sure the expected
        // exception is actually thrown.
        // =========================================================

        //[Fact]
        //public async Task CreateWithMedicalIntakeAsync_ThrowsException_WhenPatientDoesNotExist()
        //{
        //    // Arrange

        //    _patientRepositoryMock
        //        .Setup(x =>
        //            x.GetByUserIdAsync(999))
        //        .ReturnsAsync(
        //            (Patient?)null);


        //    var dto =
        //        new CreateAppointmentWithMedicalIntakeDto
        //        {
        //            AppointmentDate =
        //                new DateTime(
        //                    2026,
        //                    9,
        //                    1,
        //                    10,
        //                    0,
        //                    0),

        //            Reason =
        //                "Cardiac follow-up",

        //            Notes =
        //                "Test"
        //        };


        //    // Act + Assert

        //    var exception =
        //        await Assert.ThrowsAsync<InvalidOperationException>(
        //            () =>
        //                _service
        //                    .CreateWithMedicalIntakeAsync(
        //                        999,
        //                        dto));


        //    // Verify exception message

        //    Assert.Equal(
        //        "Patient profile was not found.",
        //        exception.Message);


        //    // Make sure no appointment was saved.

        //    Assert.Empty(
        //        await _context.Appointments.ToListAsync());


        //    _patientRepositoryMock.Verify(
        //        x =>
        //            x.GetByUserIdAsync(999),
        //        Times.Once);
        //}


        // =========================================================
        // Test 3: Error During Medical Intake Creation
        //
        // Verifies that if an exception occurs while creating
        // the medical information, the exception is propagated
        // and the transaction is rolled back.
        //
        // This test specifically verifies the:
        //
        // catch
        // {
        //     await transaction.RollbackAsync();
        //     throw;
        // }
        //
        // in the service.
        // =========================================================

        [Fact]
        public async Task CreateWithMedicalIntakeAsync_ThrowsException_WhenCreatingAllergyFails()
        {
            // Arrange

            var patient =
                new Patient
                {
                    PatientId = 5,
                    UserId = 10
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(10))
                .ReturnsAsync(patient);


            _allergyRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(
                    new List<Allergy>());


            _familyHistoryRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(
                    new List<FamilyMedicalHistory>());


            _medicationRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(
                    new List<Medication>());


            _diagnosisRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(
                    new List<Diagnosis>());


            _emergencyRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(
                    (EmergencyMedicalInformation?)null);


            var dto =
                new CreateAppointmentWithMedicalIntakeDto
                {
                    AppointmentDate =
                        new DateTime(
                            2026,
                            9,
                            1,
                            10,
                            0,
                            0),

                    Reason =
                        "Test appointment",

                    Notes =
                        "Testing exception",

                    NewAllergies =
                        new List<CreateAllergyDto>
                        {
                    new CreateAllergyDto
                    {
                        Name =
                            "Penicillin",

                        Reaction =
                            "Rash",

                        Severity =
                            "Moderate",

                        DiagnosedAt =
                            new DateTime(
                                2026,
                                8,
                                1),

                        Notes =
                            "Test allergy"
                    }
                        },

                    NewFamilyHistory =
                        new List<CreateFamilyHistoryDto>()
                };


            // Force the repository to throw an exception.

            _allergyRepositoryMock
                .Setup(x =>
                    x.CreateRangeAsync(
                        It.IsAny<List<Allergy>>()))
                .ThrowsAsync(
                    new Exception(
                        "Failed to create allergy."));


            // Act + Assert

            var exception =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .CreateWithMedicalIntakeAsync(
                                10,
                                dto));


            // Verify that the same exception was thrown.

            Assert.Equal(
                "Failed to create allergy.",
                exception.Message);


            // Verify that allergy creation was attempted.

            _allergyRepositoryMock.Verify(
                x =>
                    x.CreateRangeAsync(
                        It.IsAny<List<Allergy>>()),
                Times.Once);


            // Since the exception happened before SaveChanges,
            // the appointment must NOT exist in the database.

            Assert.Empty(
                await _context.Appointments.ToListAsync());
        }
        //// =========================================================
        //// Test 4: Create Appointment Without Existing Medical Data
        ////
        //// Verifies that:
        //// - Patient exists.
        //// - No existing medical information is available.
        //// - No new allergies are provided.
        //// - No new family history is provided.
        //// - Appointment is still created successfully.
        //// - Returned medical lists are empty.
        //// =========================================================

        //[Fact]
        //public async Task
        //    CreateWithMedicalIntakeAsync_CreatesAppointment_WhenNoMedicalDataExists()
        //{
        //    // =====================================================
        //    // Arrange
        //    // =====================================================

        //    // Create ApplicationUser
        //    var user = new ApplicationUser
        //    {
        //        Id = 20,
        //        UserName = "testuser2",
        //        NormalizedUserName = "TESTUSER2",
        //        Email = "testuser2@example.com",
        //        NormalizedEmail = "TESTUSER2@EXAMPLE.COM",
        //        EmailConfirmed = true
        //    };

        //    _context.Users.Add(user);


        //    // Create Patient
        //    var patient = new Patient
        //    {
        //        PatientId = 6,
        //        UserId = 20
        //    };

        //    _context.Patients.Add(patient);


        //    // Save User + Patient to SQLite
        //    await _context.SaveChangesAsync();


        //    // -----------------------------------------------------
        //    // Patient Repository
        //    // -----------------------------------------------------

        //    _patientRepositoryMock
        //        .Setup(x => x.GetByUserIdAsync(20))
        //        .ReturnsAsync(patient);


        //    // -----------------------------------------------------
        //    // No existing allergies
        //    // -----------------------------------------------------

        //    _allergyRepositoryMock
        //        .Setup(x => x.GetByPatientIdAsync(6))
        //        .ReturnsAsync(
        //            new List<Allergy>());


        //    // -----------------------------------------------------
        //    // No existing family history
        //    // -----------------------------------------------------

        //    _familyHistoryRepositoryMock
        //        .Setup(x => x.GetByPatientIdAsync(6))
        //        .ReturnsAsync(
        //            new List<FamilyMedicalHistory>());


        //    // -----------------------------------------------------
        //    // No existing medications
        //    // -----------------------------------------------------

        //    _medicationRepositoryMock
        //        .Setup(x => x.GetByPatientIdAsync(6))
        //        .ReturnsAsync(
        //            new List<Medication>());


        //    // -----------------------------------------------------
        //    // No existing diagnoses
        //    // -----------------------------------------------------

        //    _diagnosisRepositoryMock
        //        .Setup(x => x.GetByPatientIdAsync(6))
        //        .ReturnsAsync(
        //            new List<Diagnosis>());


        //    // -----------------------------------------------------
        //    // No emergency information
        //    // -----------------------------------------------------

        //    _emergencyRepositoryMock
        //        .Setup(x => x.GetByPatientIdAsync(6))
        //        .ReturnsAsync(
        //            (EmergencyMedicalInformation?)null);


        //    // -----------------------------------------------------
        //    // DTO
        //    // -----------------------------------------------------

        //    var dto =
        //        new CreateAppointmentWithMedicalIntakeDto
        //        {
        //            AppointmentDate =
        //                new DateTime(
        //                    2026,
        //                    9,
        //                    5,
        //                    11,
        //                    0,
        //                    0),

        //            Reason =
        //                "Regular cardiac check-up",

        //            Notes =
        //                "No previous medical information",

        //            NewAllergies =
        //                new List<CreateAllergyDto>(),

        //            NewFamilyHistory =
        //                new List<CreateFamilyHistoryDto>()
        //        };


        //    // =====================================================
        //    // Act
        //    // =====================================================

        //    var result =
        //        await _service
        //            .CreateWithMedicalIntakeAsync(
        //                20,
        //                dto);


        //    // =====================================================
        //    // Assert
        //    // =====================================================

        //    // Response exists
        //    Assert.NotNull(result);


        //    // Patient
        //    Assert.Equal(
        //        6,
        //        result.PatientId);


        //    // Appointment information
        //    Assert.Equal(
        //        dto.AppointmentDate,
        //        result.AppointmentDate);

        //    Assert.Equal(
        //        "Regular cardiac check-up",
        //        result.Reason);

        //    Assert.Equal(
        //        "No previous medical information",
        //        result.Notes);


        //    // No allergies
        //    Assert.Empty(
        //        result.Allergies);


        //    // No family history
        //    Assert.Empty(
        //        result.FamilyHistory);


        //    // No medications
        //    Assert.Empty(
        //        result.Medications);


        //    // No diagnoses
        //    Assert.Empty(
        //        result.Diagnoses);


        //    // No emergency information
        //    Assert.Null(
        //        result.EmergencyMedicalInformation);


        //    // =====================================================
        //    // Verify Repository Calls
        //    // =====================================================

        //    _patientRepositoryMock.Verify(
        //        x => x.GetByUserIdAsync(20),
        //        Times.Once);


        //    _allergyRepositoryMock.Verify(
        //        x => x.GetByPatientIdAsync(6),
        //        Times.Once);


        //    _familyHistoryRepositoryMock.Verify(
        //        x => x.GetByPatientIdAsync(6),
        //        Times.Once);


        //    _medicationRepositoryMock.Verify(
        //        x => x.GetByPatientIdAsync(6),
        //        Times.Once);


        //    _diagnosisRepositoryMock.Verify(
        //        x => x.GetByPatientIdAsync(6),
        //        Times.Once);


        //    _emergencyRepositoryMock.Verify(
        //        x => x.GetByPatientIdAsync(6),
        //        Times.Once);


        //    // =====================================================
        //    // Verify Appointment Was Saved in SQLite
        //    // =====================================================

        //    var savedAppointment =
        //        await _context.Appointments
        //            .FirstOrDefaultAsync(
        //                a =>
        //                    a.PatientId == 6
        //                    &&
        //                    a.Reason ==
        //                        "Regular cardiac check-up");


        //    Assert.NotNull(
        //        savedAppointment);


        //    Assert.Equal(
        //        Appointment.AppointmentStatus.Scheduled,
        //        savedAppointment.Status);
        //}
        //// =========================================================
        //// Test 5: Emergency Medical Information Exists
        ////
        //// Verifies that:
        //// - Patient exists.
        //// - Emergency medical information exists.
        //// - Emergency information is correctly mapped
        ////   to the response DTO.
        //// - Appointment is created successfully.
        //// =========================================================

        //[Fact]
        //public async Task
        //    CreateWithMedicalIntakeAsync_ReturnsEmergencyInformation_WhenItExists()
        //{
        //    // =====================================================
        //    // Arrange
        //    // =====================================================

        //    // Create ApplicationUser
        //    var user = new ApplicationUser
        //    {
        //        Id = 30,
        //        UserName = "testuser3",
        //        NormalizedUserName = "TESTUSER3",
        //        Email = "testuser3@example.com",
        //        NormalizedEmail = "TESTUSER3@EXAMPLE.COM",
        //        EmailConfirmed = true
        //    };

        //    _context.Users.Add(user);


        //    // Create Patient
        //    var patient = new Patient
        //    {
        //        PatientId = 7,
        //        UserId = 30
        //    };

        //    _context.Patients.Add(patient);


        //    // Save User + Patient
        //    await _context.SaveChangesAsync();


        //    // -----------------------------------------------------
        //    // Patient Repository
        //    // -----------------------------------------------------

        //    _patientRepositoryMock
        //        .Setup(x => x.GetByUserIdAsync(30))
        //        .ReturnsAsync(patient);


        //    // -----------------------------------------------------
        //    // Existing allergies
        //    // -----------------------------------------------------

        //    _allergyRepositoryMock
        //        .Setup(x => x.GetByPatientIdAsync(7))
        //        .ReturnsAsync(
        //            new List<Allergy>());


        //    // -----------------------------------------------------
        //    // Existing family history
        //    // -----------------------------------------------------

        //    _familyHistoryRepositoryMock
        //        .Setup(x => x.GetByPatientIdAsync(7))
        //        .ReturnsAsync(
        //            new List<FamilyMedicalHistory>());


        //    // -----------------------------------------------------
        //    // Existing medications
        //    // -----------------------------------------------------

        //    _medicationRepositoryMock
        //        .Setup(x => x.GetByPatientIdAsync(7))
        //        .ReturnsAsync(
        //            new List<Medication>());


        //    // -----------------------------------------------------
        //    // Existing diagnoses
        //    // -----------------------------------------------------

        //    _diagnosisRepositoryMock
        //        .Setup(x => x.GetByPatientIdAsync(7))
        //        .ReturnsAsync(
        //            new List<Diagnosis>());


        //    // -----------------------------------------------------
        //    // Emergency Medical Information
        //    // -----------------------------------------------------

        //    var emergencyInformation =
        //        new EmergencyMedicalInformation
        //        {
        //            EmergencyMedicalInformationId = 1,

        //            PatientId = 7,

        //            BloodType = "O+",

        //            PreferredHospital =
        //                "Nablus Specialty Hospital",

        //            SpecialInstructions =
        //                "Patient requires immediate cardiac monitoring.",

        //            EmergencyNotes =
        //                "History of cardiac problems."
        //        };


        //    _emergencyRepositoryMock
        //        .Setup(x => x.GetByPatientIdAsync(7))
        //        .ReturnsAsync(
        //            emergencyInformation);


        //    // -----------------------------------------------------
        //    // DTO
        //    // -----------------------------------------------------

        //    var dto =
        //        new CreateAppointmentWithMedicalIntakeDto
        //        {
        //            AppointmentDate =
        //                new DateTime(
        //                    2026,
        //                    9,
        //                    10,
        //                    12,
        //                    0,
        //                    0),

        //            Reason =
        //                "Emergency cardiac follow-up",

        //            Notes =
        //                "Patient has emergency medical information.",

        //            NewAllergies =
        //                new List<CreateAllergyDto>(),

        //            NewFamilyHistory =
        //                new List<CreateFamilyHistoryDto>()
        //        };


        //    // =====================================================
        //    // Act
        //    // =====================================================

        //    var result =
        //        await _service
        //            .CreateWithMedicalIntakeAsync(
        //                30,
        //                dto);


        //    // =====================================================
        //    // Assert
        //    // =====================================================

        //    // Response exists
        //    Assert.NotNull(result);


        //    // Patient
        //    Assert.Equal(
        //        7,
        //        result.PatientId);


        //    // Appointment
        //    Assert.Equal(
        //        dto.AppointmentDate,
        //        result.AppointmentDate);

        //    Assert.Equal(
        //        "Emergency cardiac follow-up",
        //        result.Reason);


        //    // =====================================================
        //    // Emergency Information
        //    // =====================================================

        //    Assert.NotNull(
        //        result.EmergencyMedicalInformation);


        //    Assert.Equal(
        //        1,
        //        result.EmergencyMedicalInformation
        //            .EmergencyMedicalInformationId);


        //    Assert.Equal(
        //        "O+",
        //        result.EmergencyMedicalInformation
        //            .BloodType);


        //    Assert.Equal(
        //        "Nablus Specialty Hospital",
        //        result.EmergencyMedicalInformation
        //            .PreferredHospital);


        //    Assert.Equal(
        //        "Patient requires immediate cardiac monitoring.",
        //        result.EmergencyMedicalInformation
        //            .SpecialInstructions);


        //    Assert.Equal(
        //        "History of cardiac problems.",
        //        result.EmergencyMedicalInformation
        //            .EmergencyNotes);


        //    // =====================================================
        //    // Verify Repository Calls
        //    // =====================================================

        //    _patientRepositoryMock.Verify(
        //        x => x.GetByUserIdAsync(30),
        //        Times.Once);


        //    _allergyRepositoryMock.Verify(
        //        x => x.GetByPatientIdAsync(7),
        //        Times.Once);


        //    _familyHistoryRepositoryMock.Verify(
        //        x => x.GetByPatientIdAsync(7),
        //        Times.Once);


        //    _medicationRepositoryMock.Verify(
        //        x => x.GetByPatientIdAsync(7),
        //        Times.Once);


        //    _diagnosisRepositoryMock.Verify(
        //        x => x.GetByPatientIdAsync(7),
        //        Times.Once);


        //    _emergencyRepositoryMock.Verify(
        //        x => x.GetByPatientIdAsync(7),
        //        Times.Once);


        //    // =====================================================
        //    // Verify Appointment Was Saved in SQLite
        //    // =====================================================

        //    var savedAppointment =
        //        await _context.Appointments
        //            .FirstOrDefaultAsync(
        //                a =>
        //                    a.PatientId == 7
        //                    &&
        //                    a.Reason ==
        //                        "Emergency cardiac follow-up");

 
        //    Assert.NotNull(
        //        savedAppointment);


        //    Assert.Equal(
        //        Appointment.AppointmentStatus.Scheduled,
        //        savedAppointment.Status);
        //}
    }
}