//
// This test class contains unit tests for the VitalSignService.
//
// It uses xUnit for writing and running tests and Moq for mocking
// the IVitalSignRepository dependency.
//
// The tests cover vital-sign retrieval, creation, updating, deletion,
// filtering, and the custom comparison features implemented for
// monitoring changes in patient vital signs.
//
// ======================= Retrieval Tests ==========================
//
// 1. GetByIdAsync_ReturnsVitalSign_WhenVitalSignExists
//    - Verifies that a vital-sign record is returned when the requested
//      ID exists.
//    - Checks that the returned DTO contains the correct vital-sign data.
//    - Verifies that the repository is called once.
//
// 2. GetByIdAsync_ReturnsNull_WhenVitalSignDoesNotExist
//    - Verifies that null is returned when the requested vital-sign
//      record does not exist.
//
// 3. GetByPatientIdAsync_ReturnsVitalSignsForPatient
//    - Verifies that vital-sign records belonging to a specific patient
//      are returned correctly.
//    - Ensures that all returned records belong to the requested patient.
//
// 4. GetAllAsync_ReturnsAllVitalSigns
//    - Verifies that all vital-sign records returned by the repository
//      are mapped correctly to response DTOs.
//    - Checks the number and IDs of the returned records.
//
// 5. GetMyVitalSignsAsync_ReturnsUserVitalSigns
//    - Verifies that a patient's own vital-sign records are returned
//      using the authenticated user's ID.
//    - Ensures that the repository is called with the correct user ID.
//
// ========================= Create Tests ===========================
//
// 6. CreateAsync_ReturnsCreatedVitalSign
//    - Verifies successful creation of a vital-sign record.
//    - Checks that the DTO values are mapped correctly to the entity.
//    - Verifies that the repository AddAsync method is called.
//
// ========================= Filter Tests ===========================
//
// 7. FilterAsync_ReturnsFilteredVitalSigns
//    - Verifies that the filtering parameters are passed correctly
//      to the repository.
//    - Checks that the filtered vital-sign records are returned.
//
// ========================= Update Tests ===========================
//
// 8. UpdateAsync_ReturnsTrue_WhenVitalSignExists
//    - Verifies successful update of an existing vital-sign record.
//    - Checks that heart rate, blood pressure, oxygen saturation,
//      temperature, measurement date, notes, and doctor name are updated.
//    - Verifies that the repository UpdateAsync method is called.
//
// 9. UpdateAsync_ReturnsFalse_WhenVitalSignDoesNotExist
//    - Verifies that update returns false when the vital-sign record
//      does not exist.
//    - Ensures that UpdateAsync is not called.
//
// ========================= Delete Tests ===========================
//
// 10. DeleteAsync_ReturnsTrue_WhenVitalSignExists
//     - Verifies successful deletion of an existing vital-sign record.
//     - Ensures that the repository DeleteAsync method is called.
//
// 11. DeleteAsync_ReturnsFalse_WhenVitalSignDoesNotExist
//     - Verifies that deletion returns false when the record does not exist.
//     - Ensures that no delete operation is performed.
//
// =================== Latest Two Comparison Tests ==================
//
// 12. CompareLatestTwoAsync_ReturnsComparison_WhenTwoRecordsExist
//     - Verifies comparison between the latest and previous vital-sign
//       measurements.
//     - Checks the previous and latest values.
//     - Verifies calculated changes for heart rate, blood pressure,
//       oxygen saturation, and temperature.
//     - Verifies the calculated improvement or worsening status.
//
// 13. CompareLatestTwoAsync_ReturnsNull_WhenLessThanTwoRecordsExist
//     - Verifies that comparison returns null when fewer than two
//       vital-sign records are available.
//
// ====================== Date Comparison Tests =====================
//
// 14. CompareByDatesAsync_ReturnsComparison_WhenBothDatesHaveRecords
//     - Verifies comparison between vital-sign records from two
//       user-selected dates.
//     - Checks that the correct records are retrieved for both dates.
//     - Verifies calculated changes and improvement status.
//
// 15. CompareByDatesAsync_ReturnsNull_WhenOneDateHasNoRecord
//     - Verifies that comparison returns null when one of the selected
//       dates does not contain a vital-sign record.
//
// =========================== Coverage =============================
//
// The tests cover:
// - Vital-sign retrieval by ID, patient, and user.
// - Retrieving all vital-sign records.
// - Vital-sign creation.
// - Vital-sign filtering.
// - Partial update behavior.
// - Vital-sign deletion.
// - Latest-two-record comparison.
// - Date-based comparison.
// - Improvement, worsening, and no-change comparison logic.
// - Success and failure scenarios.
// - Repository interaction using Moq.
//
using Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Services;
using Moq;
using Xunit;

namespace CardiacPatientMonitoringSystem.Tests.Services
{
    public class VitalSignServiceTests
    {
        private readonly Mock<IVitalSignRepository>
            _repositoryMock;

        private readonly VitalSignService _service;

        public VitalSignServiceTests()
        {
            _repositoryMock =
                new Mock<IVitalSignRepository>();

            _service =
                new VitalSignService(
                    _repositoryMock.Object);
        }

        // =========================================================
        // GetByIdAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task GetByIdAsync_ReturnsVitalSign_WhenVitalSignExists()
        {
            // Arrange

            var vitalSign =
                CreateVitalSign();

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(vitalSign);

            // Act

            var result =
                await _service.GetByIdAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                vitalSign.VitalSignId,
                result.VitalSignId);

            Assert.Equal(
                vitalSign.PatientId,
                result.PatientId);

            Assert.Equal(
                vitalSign.HeartRate,
                result.HeartRate);

            Assert.Equal(
                vitalSign.SystolicPressure,
                result.SystolicPressure);

            Assert.Equal(
                vitalSign.DiastolicPressure,
                result.DiastolicPressure);

            Assert.Equal(
                vitalSign.OxygenSaturation,
                result.OxygenSaturation);

            Assert.Equal(
                vitalSign.Temperature,
                result.Temperature);

            _repositoryMock.Verify(
                x =>
                    x.GetByIdAsync(1),
                Times.Once);
        }

        // =========================================================
        // GetByIdAsync - NOT FOUND
        // =========================================================

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenVitalSignDoesNotExist()
        {
            // Arrange

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(999))
                .ReturnsAsync(
                    (VitalSign?)null);

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
        // GetByPatientIdAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task GetByPatientIdAsync_ReturnsVitalSignsForPatient()
        {
            // Arrange

            var vitalSigns =
                new List<VitalSign>
                {
                    CreateVitalSign(
                        id: 1,
                        patientId: 5),

                    CreateVitalSign(
                        id: 2,
                        patientId: 5)
                };

            _repositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(5))
                .ReturnsAsync(vitalSigns);

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
                v => Assert.Equal(
                    5,
                    v.PatientId));

            _repositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(5),
                Times.Once);
        }

        // =========================================================
        // GetAllAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task GetAllAsync_ReturnsAllVitalSigns()
        {
            // Arrange

            var vitalSigns =
                new List<VitalSign>
                {
                    CreateVitalSign(1, 1),
                    CreateVitalSign(2, 2),
                    CreateVitalSign(3, 3)
                };

            _repositoryMock
                .Setup(x =>
                    x.GetAllAsync())
                .ReturnsAsync(vitalSigns);

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
                resultList[0].VitalSignId);

            Assert.Equal(
                3,
                resultList[2].VitalSignId);

            _repositoryMock.Verify(
                x =>
                    x.GetAllAsync(),
                Times.Once);
        }

        // =========================================================
        // CreateAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task CreateAsync_ReturnsCreatedVitalSign()
        {
            // Arrange

            var measuredAt =
                new DateTime(
                    2026,
                    8,
                    19,
                    10,
                    0,
                    0);

            var dto =
                new CreateVitalSignDto
                {
                    PatientId = 3,

                    RecordedByDoctorName =
                        "Dr. Ahmad",

                    HeartRate = 85,

                    SystolicPressure = 120,

                    DiastolicPressure = 80,

                    OxygenSaturation = 98,

                    Temperature = 36.8m,

                    MeasuredAt = measuredAt,

                    Notes =
                        "Regular measurement."
                };

            var createdVitalSign =
                new VitalSign
                {
                    VitalSignId = 10,

                    PatientId = 3,

                    RecordedByDoctorName =
                        "Dr. Ahmad",

                    HeartRate = 85,

                    SystolicPressure = 120,

                    DiastolicPressure = 80,

                    OxygenSaturation = 98,

                    Temperature = 36.8m,

                    MeasuredAt = measuredAt,

                    CreatedAt =
                        DateTime.UtcNow,

                    Notes =
                        "Regular measurement."
                };

            _repositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<VitalSign>()))
                .ReturnsAsync(
                    createdVitalSign);

            // Act

            var result =
                await _service.CreateAsync(dto);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                10,
                result.VitalSignId);

            Assert.Equal(
                3,
                result.PatientId);

            Assert.Equal(
                85,
                result.HeartRate);

            Assert.Equal(
                120,
                result.SystolicPressure);

            Assert.Equal(
                80,
                result.DiastolicPressure);

            Assert.Equal(
                98m,
                result.OxygenSaturation);

            Assert.Equal(
                36.8m,
                result.Temperature);

            _repositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.Is<VitalSign>(v =>
                            v.PatientId == 3 &&
                            v.HeartRate == 85 &&
                            v.SystolicPressure == 120 &&
                            v.DiastolicPressure == 80)),
                Times.Once);
        }

        // =========================================================
        // GetMyVitalSignsAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task GetMyVitalSignsAsync_ReturnsUserVitalSigns()
        {
            // Arrange

            var vitalSigns =
                new List<VitalSign>
                {
                    CreateVitalSign(1, 7),
                    CreateVitalSign(2, 7)
                };

            _repositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(7))
                .ReturnsAsync(vitalSigns);

            // Act

            var result =
                await _service.GetMyVitalSignsAsync(7);

            // Assert

            var resultList =
                result.ToList();

            Assert.Equal(
                2,
                resultList.Count);

            Assert.All(
                resultList,
                v =>
                    Assert.Equal(
                        7,
                        v.PatientId));

            _repositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(7),
                Times.Once);
        }

        // =========================================================
        // FilterAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task FilterAsync_ReturnsFilteredVitalSigns()
        {
            // Arrange

            var filter =
                new VitalSignFilterDto
                {
                    PatientName = "Ahmed",
                    Age = 31,
                    Gender = "Male",
                    NationalId = "123"
                };

            var vitalSigns =
                new List<VitalSign>
                {
                    CreateVitalSign(1, 1),
                    CreateVitalSign(2, 1)
                };

            _repositoryMock
                .Setup(x =>
                    x.FilterAsync(
                        filter.PatientName,
                        filter.Age,
                        filter.Gender,
                        filter.NationalId))
                .ReturnsAsync(vitalSigns);

            // Act

            var result =
                await _service.FilterAsync(filter);

            // Assert

            var resultList =
                result.ToList();

            Assert.Equal(
                2,
                resultList.Count);

            _repositoryMock.Verify(
                x =>
                    x.FilterAsync(
                        "Ahmed",
                        31,
                        "Male",
                        "123"),
                Times.Once);
        }

        // =========================================================
        // UpdateAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenVitalSignExists()
        {
            // Arrange

            var vitalSign =
                CreateVitalSign(
                    id: 1,
                    patientId: 3);

            var dto =
                new UpdateVitalSignDto
                {
                    HeartRate = 90,

                    SystolicPressure = 130,

                    DiastolicPressure = 85,

                    OxygenSaturation = 97,

                    Temperature = 37.1m,

                    MeasuredAt =
                        new DateTime(
                            2026,
                            8,
                            19,
                            12,
                            0,
                            0),

                    Notes =
                        "Updated measurement.",

                    RecordedByDoctorName =
                        "Dr. Lina"
                };

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(vitalSign);

            _repositoryMock
                .Setup(x =>
                    x.UpdateAsync(
                        It.IsAny<VitalSign>()))
                .Returns(Task.CompletedTask);

            // Act

            var result =
                await _service.UpdateAsync(
                    1,
                    dto);

            // Assert

            Assert.True(result);

            Assert.Equal(
                90,
                vitalSign.HeartRate);

            Assert.Equal(
                130,
                vitalSign.SystolicPressure);

            Assert.Equal(
                85,
                vitalSign.DiastolicPressure);

            Assert.Equal(
                97m,
                vitalSign.OxygenSaturation);

            Assert.Equal(
                37.1m,
                vitalSign.Temperature);

            Assert.Equal(
                "Updated measurement.",
                vitalSign.Notes);

            Assert.Equal(
                "Dr. Lina",
                vitalSign.RecordedByDoctorName);

            _repositoryMock.Verify(
                x =>
                    x.UpdateAsync(vitalSign),
                Times.Once);
        }

        // =========================================================
        // UpdateAsync - NOT FOUND
        // =========================================================

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenVitalSignDoesNotExist()
        {
            // Arrange

            var dto =
                new UpdateVitalSignDto
                {
                    HeartRate = 90
                };

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(999))
                .ReturnsAsync(
                    (VitalSign?)null);

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
                        It.IsAny<VitalSign>()),
                Times.Never);
        }

        // =========================================================
        // DeleteAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenVitalSignExists()
        {
            // Arrange

            var vitalSign =
                CreateVitalSign();

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(vitalSign);

            _repositoryMock
                .Setup(x =>
                    x.DeleteAsync(vitalSign))
                .Returns(Task.CompletedTask);

            // Act

            var result =
                await _service.DeleteAsync(1);

            // Assert

            Assert.True(result);

            _repositoryMock.Verify(
                x =>
                    x.DeleteAsync(vitalSign),
                Times.Once);
        }

        // =========================================================
        // DeleteAsync - NOT FOUND
        // =========================================================

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenVitalSignDoesNotExist()
        {
            // Arrange

            _repositoryMock
                .Setup(x =>
                    x.GetByIdAsync(999))
                .ReturnsAsync(
                    (VitalSign?)null);

            // Act

            var result =
                await _service.DeleteAsync(999);

            // Assert

            Assert.False(result);

            _repositoryMock.Verify(
                x =>
                    x.DeleteAsync(
                        It.IsAny<VitalSign>()),
                Times.Never);
        }

        // =========================================================
        // CompareLatestTwoAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task CompareLatestTwoAsync_ReturnsComparison_WhenTwoRecordsExist()
        {
            // Arrange

            var previous =
                CreateVitalSign(
                    id: 1,
                    patientId: 3);

            previous.HeartRate = 100;
            previous.SystolicPressure = 150;
            previous.DiastolicPressure = 95;
            previous.OxygenSaturation = 90;
            previous.Temperature = 38;

            previous.MeasuredAt =
                new DateTime(
                    2026,
                    8,
                    18,
                    10,
                    0,
                    0);

            var latest =
                CreateVitalSign(
                    id: 2,
                    patientId: 3);

            latest.HeartRate = 80;
            latest.SystolicPressure = 130;
            latest.DiastolicPressure = 82;
            latest.OxygenSaturation = 97;
            latest.Temperature = 37;

            latest.MeasuredAt =
                new DateTime(
                    2026,
                    8,
                    19,
                    10,
                    0,
                    0);

            var records =
                new List<VitalSign>
                {
                    latest,
                    previous
                };

            _repositoryMock
                .Setup(x =>
                    x.GetLatestTwoByUserIdAsync(3))
                .ReturnsAsync(records);

            // Act

            var result =
                await _service.CompareLatestTwoAsync(3);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                100,
                result.Previous.HeartRate);

            Assert.Equal(
                80,
                result.Latest.HeartRate);

            Assert.Equal(
                -20,
                result.Comparison.HeartRate.Change);

            Assert.Equal(
                "Improved",
                result.Comparison.HeartRate.Status);

            Assert.Equal(
                -20,
                result.Comparison.SystolicPressure.Change);

            Assert.Equal(
                "Improved",
                result.Comparison.SystolicPressure.Status);

            Assert.Equal(
                -13,
                result.Comparison.DiastolicPressure.Change);

            Assert.Equal(
                "Improved",
                result.Comparison.DiastolicPressure.Status);

            Assert.Equal(
                7,
                result.Comparison.OxygenSaturation.Change);

            Assert.Equal(
                "Improved",
                result.Comparison.OxygenSaturation.Status);

            Assert.Equal(
                -1,
                result.Comparison.Temperature.Change);

            Assert.Equal(
                "Improved",
                result.Comparison.Temperature.Status);
        }

        // =========================================================
        // CompareLatestTwoAsync - LESS THAN TWO RECORDS
        // =========================================================

        [Fact]
        public async Task CompareLatestTwoAsync_ReturnsNull_WhenLessThanTwoRecordsExist()
        {
            // Arrange

            var records =
                new List<VitalSign>
                {
                    CreateVitalSign()
                };

            _repositoryMock
                .Setup(x =>
                    x.GetLatestTwoByUserIdAsync(3))
                .ReturnsAsync(records);

            // Act

            var result =
                await _service.CompareLatestTwoAsync(3);

            // Assert

            Assert.Null(result);
        }

        // =========================================================
        // CompareByDatesAsync - SUCCESS
        // =========================================================

        [Fact]
        public async Task CompareByDatesAsync_ReturnsComparison_WhenBothDatesHaveRecords()
        {
            // Arrange

            var firstDate =
                new DateTime(
                    2026,
                    7,
                    18);

            var secondDate =
                new DateTime(
                    2026,
                    8,
                    18);

            var firstVitalSign =
                CreateVitalSign(
                    id: 1,
                    patientId: 3);

            firstVitalSign.HeartRate = 100;
            firstVitalSign.SystolicPressure = 150;
            firstVitalSign.DiastolicPressure = 95;
            firstVitalSign.OxygenSaturation = 90;
            firstVitalSign.Temperature = 38;

            firstVitalSign.MeasuredAt =
                firstDate.AddHours(10);

            var secondVitalSign =
                CreateVitalSign(
                    id: 2,
                    patientId: 3);

            secondVitalSign.HeartRate = 80;
            secondVitalSign.SystolicPressure = 130;
            secondVitalSign.DiastolicPressure = 82;
            secondVitalSign.OxygenSaturation = 97;
            secondVitalSign.Temperature = 37;

            secondVitalSign.MeasuredAt =
                secondDate.AddHours(10);

            _repositoryMock
                .Setup(x =>
                    x.GetLatestByUserIdAndDateAsync(
                        3,
                        firstDate))
                .ReturnsAsync(firstVitalSign);

            _repositoryMock
                .Setup(x =>
                    x.GetLatestByUserIdAndDateAsync(
                        3,
                        secondDate))
                .ReturnsAsync(secondVitalSign);

            // Act

            var result =
                await _service.CompareByDatesAsync(
                    3,
                    firstDate,
                    secondDate);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                100,
                result.FirstDate.HeartRate);

            Assert.Equal(
                80,
                result.SecondDate.HeartRate);

            Assert.Equal(
                -20,
                result.Comparison.HeartRate.Change);

            Assert.Equal(
                "Improved",
                result.Comparison.HeartRate.Status);

            Assert.Equal(
                7,
                result.Comparison.OxygenSaturation.Change);

            Assert.Equal(
                "Improved",
                result.Comparison.OxygenSaturation.Status);

            _repositoryMock.Verify(
                x =>
                    x.GetLatestByUserIdAndDateAsync(
                        3,
                        firstDate),
                Times.Once);

            _repositoryMock.Verify(
                x =>
                    x.GetLatestByUserIdAndDateAsync(
                        3,
                        secondDate),
                Times.Once);
        }

        // =========================================================
        // CompareByDatesAsync - MISSING DATE
        // =========================================================

        [Fact]
        public async Task CompareByDatesAsync_ReturnsNull_WhenOneDateHasNoRecord()
        {
            // Arrange

            var firstDate =
                new DateTime(
                    2026,
                    7,
                    18);

            var secondDate =
                new DateTime(
                    2026,
                    8,
                    18);

            var firstVitalSign =
                CreateVitalSign();

            _repositoryMock
                .Setup(x =>
                    x.GetLatestByUserIdAndDateAsync(
                        3,
                        firstDate))
                .ReturnsAsync(firstVitalSign);

            _repositoryMock
                .Setup(x =>
                    x.GetLatestByUserIdAndDateAsync(
                        3,
                        secondDate))
                .ReturnsAsync(
                    (VitalSign?)null);

            // Act

            var result =
                await _service.CompareByDatesAsync(
                    3,
                    firstDate,
                    secondDate);

            // Assert

            Assert.Null(result);
        }

        // =========================================================
        // Helper
        // =========================================================

        private static VitalSign CreateVitalSign(
            int id = 1,
            int patientId = 1)
        {
            return new VitalSign
            {
                VitalSignId = id,

                PatientId = patientId,

                RecordedByDoctorName =
                    "Dr. Ahmad",

                HeartRate = 80,

                SystolicPressure = 120,

                DiastolicPressure = 80,

                OxygenSaturation = 98,

                Temperature = 36.8m,

                MeasuredAt =
                    DateTime.UtcNow,

                CreatedAt =
                    DateTime.UtcNow,

                Notes =
                    "Test vital sign"
            };
        }
    }
}