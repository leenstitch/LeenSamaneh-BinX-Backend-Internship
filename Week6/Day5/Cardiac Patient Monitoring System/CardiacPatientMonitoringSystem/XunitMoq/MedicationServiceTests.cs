using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Services;
using Moq;
using Xunit;

namespace CardiacPatientMonitoringSystem.XunitMoq
{
    // ================================================================
    // MedicationService Unit Tests
    // ================================================================
    //
    // This test class contains unit tests for the MedicationService.
    //
    // Testing tools:
    // - xUnit: Used to create and run the tests.
    // - Moq: Used to mock the IMedicationRepository dependency.
    //
    // The tests cover:
    //
    // Retrieval:
    // 1. Get a medication by ID when it exists.
    // 2. Return null when a medication does not exist.
    // 3. Get medications for a specific patient.
    // 4. Get medications for the authenticated patient.
    // 5. Get all medications.
    //
    // Creation:
    // 6. Successfully create a medication for the authenticated patient.
    // 7. Return null when the user is not linked to a patient.
    //
    // Updating:
    // 8. Successfully update an existing medication.
    // 9. Return false when the medication does not exist.
    //
    // Deletion:
    // 10. Successfully delete an existing medication.
    // 11. Return false when the medication does not exist.
    //
    // Filtering:
    // 12. Pass the medication filter to the repository and return
    //     the filtered medications.
    //
    // The tests also verify:
    // - Correct mapping between Medication entities and DTOs.
    // - Correct patient identification using the authenticated user ID.
    // - Correct interaction with the repository.
    // - Success and failure scenarios.
    //
    // ================================================================

    public class MedicationServiceTests
    {
        private readonly Mock<IMedicationRepository>
            _repositoryMock;

        private readonly MedicationService _service;

        public MedicationServiceTests()
        {
            _repositoryMock =
                new Mock<IMedicationRepository>();

            _service =
                new MedicationService(
                    _repositoryMock.Object);
        }

        // =========================================================
        // Test 1: Get Medication By ID - Success
        // Verifies that a medication is returned when the requested
        // medication ID exists and that its data is mapped correctly.
        // =========================================================

        [Fact]
        public async Task GetByIdAsync_ReturnsMedication_WhenMedicationExists()
        {
            // Arrange

            var medication =
                CreateMedication();

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(medication);

            // Act

            var result =
                await _service.GetByIdAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                medication.MedicationId,
                result.MedicationId);

            Assert.Equal(
                medication.PatientId,
                result.PatientId);

            Assert.Equal(
                medication.Name,
                result.Name);

            Assert.Equal(
                medication.Dosage,
                result.Dosage);

            Assert.Equal(
                medication.Frequency,
                result.Frequency);

            Assert.Equal(
                medication.PrescribedByDoctorName,
                result.PrescribedByDoctorName);

            Assert.Equal(
                medication.PrescribedBySpecialization,
                result.PrescribedBySpecialization);

            _repositoryMock.Verify(
                x =>
                    x.GetByIdAsync(1),
                Times.Once);
        }

        // =========================================================
        // Test 2: Get Medication By ID - Not Found
        // Verifies that null is returned when the requested
        // medication does not exist.
        // =========================================================

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenMedicationDoesNotExist()
        {
            // Arrange

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(999))
                .ReturnsAsync(
                    (Medication?)null);

            // Act

            var result =
                await _service.GetByIdAsync(999);

            // Assert

            Assert.Null(result);

            _repositoryMock.Verify(
                x =>
                    x.GetByIdAsync(999),
                Times.Once);
        }

        // =========================================================
        // Test 3: Get Medications By Patient
        // Verifies that medications belonging to a specific patient
        // are returned correctly.
        // =========================================================

        [Fact]
        public async Task GetByPatientIdAsync_ReturnsPatientMedications()
        {
            // Arrange

            var medications =
                new List<Medication>
                {
                    CreateMedication(
                        id: 1,
                        patientId: 5),

                    CreateMedication(
                        id: 2,
                        patientId: 5)
                };

            _repositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(
                    medications);

            // Act

            var result =
                await _service.GetByPatientIdAsync(5);

            // Assert

            var resultList =
                result.ToList();

            Assert.Equal(
                2,
                resultList.Count);

            Assert.All(
                resultList,
                medication =>
                    Assert.Equal(
                        5,
                        medication.PatientId));

            _repositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(5),
                Times.Once);
        }

        // =========================================================
        // Test 4: Get My Medications
        // Verifies that medications belonging to the authenticated
        // user's patient profile are returned correctly.
        // =========================================================

        [Fact]
        public async Task GetMyMedicationsAsync_ReturnsUserMedications()
        {
            // Arrange

            var medications =
                new List<Medication>
                {
                    CreateMedication(1, 7),
                    CreateMedication(2, 7)
                };

            _repositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(7))
                .ReturnsAsync(
                    medications);

            // Act

            var result =
                await _service.GetMyMedicationsAsync(7);

            // Assert

            var resultList =
                result.ToList();

            Assert.Equal(
                2,
                resultList.Count);

            Assert.All(
                resultList,
                medication =>
                    Assert.Equal(
                        7,
                        medication.PatientId));

            _repositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(7),
                Times.Once);
        }

        // =========================================================
        // Test 5: Get All Medications
        // Verifies that all medications returned by the repository
        // are mapped and returned correctly.
        // =========================================================

        [Fact]
        public async Task GetAllAsync_ReturnsAllMedications()
        {
            // Arrange

            var medications =
                new List<Medication>
                {
                    CreateMedication(1, 1),
                    CreateMedication(2, 2),
                    CreateMedication(3, 3)
                };

            _repositoryMock
                .Setup(x =>
                    x.GetAllAsync())
                .ReturnsAsync(
                    medications);

            // Act

            var result =
                await _service.GetAllAsync();

            // Assert

            var resultList =
                result.ToList();

            Assert.Equal(
                3,
                resultList.Count);

            Assert.Equal(
                1,
                resultList[0].MedicationId);

            Assert.Equal(
                3,
                resultList[2].MedicationId);

            _repositoryMock.Verify(
                x =>
                    x.GetAllAsync(),
                Times.Once);
        }

        // =========================================================
        // Test 6: Create Medication - Success
        // Verifies that a medication is created successfully for
        // the patient linked to the authenticated user.
        // =========================================================

        [Fact]
        public async Task CreateAsync_ReturnsCreatedMedication_WhenUserHasPatient()
        {
            // Arrange

            const int userId = 10;
            const int patientId = 5;

            var dto =
                new CreateMedicationDto
                {
                    PrescribedByDoctorName =
                        "Dr. Ahmad Hassan",

                    PrescribedBySpecialization =
                        "Cardiology",

                    Name =
                        "Aspirin",

                    Dosage =
                        "100 mg",

                    Frequency =
                        "Once daily",

                    StartDate =
                        new DateTime(
                            2026,
                            8,
                            1),

                    EndDate =
                        new DateTime(
                            2026,
                            9,
                            1),

                    Notes =
                        "Take after breakfast."
                };

            var createdMedication =
                new Medication
                {
                    MedicationId = 20,

                    PatientId =
                        patientId,

                    PrescribedByDoctorName =
                        dto.PrescribedByDoctorName,

                    PrescribedBySpecialization =
                        dto.PrescribedBySpecialization,

                    Name =
                        dto.Name,

                    Dosage =
                        dto.Dosage,

                    Frequency =
                        dto.Frequency,

                    StartDate =
                        dto.StartDate,

                    EndDate =
                        dto.EndDate,

                    Notes =
                        dto.Notes,

                    CreatedAt =
                        DateTime.UtcNow,

                    UpdatedAt =
                        DateTime.UtcNow
                };

            _repositoryMock
                .Setup(x =>
                    x.GetPatientIdByUserIdAsync(userId))
                .ReturnsAsync(
                    patientId);

            _repositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<Medication>()))
                .ReturnsAsync(
                    createdMedication);

            // Act

            var result =
                await _service.CreateAsync(
                    userId,
                    dto);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                20,
                result.MedicationId);

            Assert.Equal(
                patientId,
                result.PatientId);

            Assert.Equal(
                "Aspirin",
                result.Name);

            Assert.Equal(
                "100 mg",
                result.Dosage);

            Assert.Equal(
                "Once daily",
                result.Frequency);

            Assert.Equal(
                "Dr. Ahmad Hassan",
                result.PrescribedByDoctorName);

            Assert.Equal(
                "Cardiology",
                result.PrescribedBySpecialization);

            _repositoryMock.Verify(
                x =>
                    x.GetPatientIdByUserIdAsync(userId),
                Times.Once);

            _repositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.Is<Medication>(m =>
                            m.PatientId == patientId &&
                            m.Name == "Aspirin" &&
                            m.Dosage == "100 mg" &&
                            m.Frequency == "Once daily")),
                Times.Once);
        }

        // =========================================================
        // Test 7: Create Medication - Patient Not Found
        // Verifies that medication creation returns null when the
        // authenticated user is not linked to a patient.
        // Ensures that AddAsync is not called.
        // =========================================================

        [Fact]
        public async Task CreateAsync_ReturnsNull_WhenUserHasNoPatient()
        {
            // Arrange

            const int userId = 999;

            var dto =
                new CreateMedicationDto
                {
                    PrescribedByDoctorName =
                        "Dr. Ahmad",

                    PrescribedBySpecialization =
                        "Cardiology",

                    Name =
                        "Aspirin",

                    Dosage =
                        "100 mg",

                    Frequency =
                        "Once daily",

                    StartDate =
                        DateTime.Today
                };

            _repositoryMock
                .Setup(x =>
                    x.GetPatientIdByUserIdAsync(userId))
                .ReturnsAsync(
                    (int?)null);

            // Act

            var result =
                await _service.CreateAsync(
                    userId,
                    dto);

            // Assert

            Assert.Null(result);

            _repositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<Medication>()),
                Times.Never);
        }

        // =========================================================
        // Test 8: Update Medication - Success
        // Verifies that an existing medication is updated with the
        // supplied values and that the repository update method
        // is called.
        // =========================================================

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenMedicationExists()
        {
            // Arrange

            var medication =
                CreateMedication();

            var dto =
                new UpdateMedicationDto
                {
                    PrescribedByDoctorName =
                        "Dr. Lina Khaled",

                    PrescribedBySpecialization =
                        "Cardiology",

                    Name =
                        "Bisoprolol",

                    Dosage =
                        "5 mg",

                    Frequency =
                        "Once daily",

                    StartDate =
                        new DateTime(
                            2026,
                            8,
                            10),

                    EndDate =
                        new DateTime(
                            2026,
                            9,
                            10),

                    Notes =
                        "Monitor heart rate."
                };

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(
                    medication);

            _repositoryMock
                .Setup(x =>
                    x.UpdateAsync(
                        It.IsAny<Medication>()))
                .Returns(
                    Task.CompletedTask);

            // Act

            var result =
                await _service.UpdateAsync(
                    1,
                    dto);

            // Assert

            Assert.True(result);

            Assert.Equal(
                "Bisoprolol",
                medication.Name);

            Assert.Equal(
                "5 mg",
                medication.Dosage);

            Assert.Equal(
                "Once daily",
                medication.Frequency);

            Assert.Equal(
                "Dr. Lina Khaled",
                medication.PrescribedByDoctorName);

            Assert.Equal(
                "Cardiology",
                medication.PrescribedBySpecialization);

            Assert.Equal(
                new DateTime(
                    2026,
                    8,
                    10),
                medication.StartDate);

            Assert.Equal(
                new DateTime(
                    2026,
                    9,
                    10),
                medication.EndDate);

            Assert.Equal(
                "Monitor heart rate.",
                medication.Notes);

            _repositoryMock.Verify(
                x =>
                    x.UpdateAsync(medication),
                Times.Once);
        }

        // =========================================================
        // Test 9: Update Medication - Not Found
        // Verifies that update returns false when the medication
        // does not exist and no update operation is performed.
        // =========================================================

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenMedicationDoesNotExist()
        {
            // Arrange

            var dto =
                new UpdateMedicationDto
                {
                    Name =
                        "Aspirin"
                };

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(999))
                .ReturnsAsync(
                    (Medication?)null);

            // Act

            var result =
                await _service.UpdateAsync(
                    999,
                    dto);

            // Assert

            Assert.False(result);

            _repositoryMock.Verify(
                x =>
                    x.UpdateAsync(
                        It.IsAny<Medication>()),
                Times.Never);
        }

        // =========================================================
        // Test 10: Delete Medication - Success
        // Verifies that an existing medication is deleted
        // successfully.
        // =========================================================

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenMedicationExists()
        {
            // Arrange

            var medication =
                CreateMedication();

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(
                    medication);

            _repositoryMock
                .Setup(x =>
                    x.DeleteAsync(
                        medication))
                .Returns(
                    Task.CompletedTask);

            // Act

            var result =
                await _service.DeleteAsync(1);

            // Assert

            Assert.True(result);

            _repositoryMock.Verify(
                x =>
                    x.DeleteAsync(
                        medication),
                Times.Once);
        }

        // =========================================================
        // Test 11: Delete Medication - Not Found
        // Verifies that delete returns false when the medication
        // does not exist.
        // =========================================================

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenMedicationDoesNotExist()
        {
            // Arrange

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(999))
                .ReturnsAsync(
                    (Medication?)null);

            // Act

            var result =
                await _service.DeleteAsync(999);

            // Assert

            Assert.False(result);

            _repositoryMock.Verify(
                x =>
                    x.DeleteAsync(
                        It.IsAny<Medication>()),
                Times.Never);
        }

        // =========================================================
        // Test 12: Filter My Medications
        // Verifies that the medication filter is passed correctly
        // to the repository and that the filtered medications are
        // returned as response DTOs.
        //
        // Note:
        // Active / Expired / Upcoming filtering logic itself is
        // implemented inside MedicationRepository, so this service
        // test verifies the service-to-repository interaction.
        // =========================================================

        [Fact]
        public async Task FilterMyMedicationsAsync_ReturnsFilteredMedications()
        {
            // Arrange

            const int userId = 10;

            var filter =
                new MedicationFilterDto
                {
                    Name =
                        "Aspirin",

                    Status =
                        "Expired",

                    StartDate =
                        new DateTime(
                            2026,
                            6,
                            1),

                    EndDate =
                        new DateTime(
                            2026,
                            7,
                            1)
                };

            var medications =
                new List<Medication>
                {
                    CreateMedication(
                        id: 1,
                        patientId: 5),

                    CreateMedication(
                        id: 2,
                        patientId: 5)
                };

            _repositoryMock
                .Setup(x =>
                    x.FilterByUserIdAsync(
                        userId,
                        filter))
                .ReturnsAsync(
                    medications);

            // Act

            var result =
                await _service.FilterMyMedicationsAsync(
                    userId,
                    filter);

            // Assert

            var resultList =
                result.ToList();

            Assert.Equal(
                2,
                resultList.Count);

            _repositoryMock.Verify(
                x =>
                    x.FilterByUserIdAsync(
                        userId,
                        filter),
                Times.Once);

            Assert.Equal(
                medications[0].MedicationId,
                resultList[0].MedicationId);

            Assert.Equal(
                medications[1].MedicationId,
                resultList[1].MedicationId);
        }

        // =========================================================
        // Helper Method
        // =========================================================

        private static Medication CreateMedication(
            int id = 1,
            int patientId = 1)
        {
            return new Medication
            {
                MedicationId =
                    id,

                PatientId =
                    patientId,

                PrescribedByDoctorName =
                    "Dr. Ahmad Hassan",

                PrescribedBySpecialization =
                    "Cardiology",

                Name =
                    "Aspirin",

                Dosage =
                    "100 mg",

                Frequency =
                    "Once daily",

                StartDate =
                    new DateTime(
                        2026,
                        8,
                        1),

                EndDate =
                    new DateTime(
                        2026,
                        9,
                        1),

                Notes =
                    "Test medication.",

                CreatedAt =
                    DateTime.UtcNow,

                UpdatedAt =
                    DateTime.UtcNow
            };
        }
    }
}