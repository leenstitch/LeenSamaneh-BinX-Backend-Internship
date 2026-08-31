// ================================================================
// VitalSignService Unit Tests
// ================================================================
//
// This test class contains unit tests for the VitalSignService.
//
// Testing tool:
// - xUnit: Used to create and run the tests.
// - Moq: Used to mock the IVitalSignRepository dependency.
//
// The tests cover:
//
// Retrieval:
// 1. Get a vital sign by ID when it exists.
// 2. Return null when a vital sign does not exist.
// 3. Get vital signs for a specific patient.
// 4. Get all vital signs.
// 5. Get the authenticated patient's vital signs.
//
// Creation:
// 6. Successfully create a vital-sign record.
//
// Filtering:
// 7. Filter vital signs using patient information.
//
// Updating:
// 8. Successfully update an existing vital-sign record.
// 9. Return false when the vital-sign record does not exist.
//
// Deletion:
// 10. Successfully delete an existing vital-sign record.
// 11. Return false when the vital-sign record does not exist.
//
// Comparison:
// 12. Compare the latest two vital-sign records.
// 13. Return null when fewer than two records exist.
// 14. Compare vital signs recorded on two selected dates.
// 15. Return null when one of the selected dates has no record.
//
// The tests also verify:
// - Correct mapping from entities to DTOs.
// - Correct repository interaction.
// - Update and delete behavior.
// - Vital-sign comparison calculations.
// - Improved, worsened, and unchanged statuses.
//
// ================================================================
using Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Services;
using Moq;
using Xunit;

namespace CardiacPatientMonitoringSystem.XunitMoq
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
        // Test 1: Get Vital Sign By ID - Success
        // Verifies that a vital-sign record is returned when the
        // requested ID exists and that its data is mapped correctly.
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
        // Test 2: Get Vital Sign By ID - Not Found
        // Verifies that null is returned when the requested
        // vital-sign record does not exist.
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
        // Test 3: Get Vital Signs By Patient
        // Verifies that vital-sign records belonging to a specific
        // patient are returned correctly.
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
                v =>
                    Assert.Equal(
                        5,
                        v.PatientId));

            _repositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(5),
                Times.Once);
        }

        // =========================================================
        // Test 4: Get All Vital Signs
        // Verifies that all vital-sign records returned by the
        // repository are mapped and returned correctly.
        // =========================================================

        //////////[Fact]
        //////////public async Task GetAllAsync_ReturnsAllVitalSigns()
        //////////{
        //////////    // Arrange

        //////////    var vitalSigns =
        //////////        new List<VitalSign>
        //////////        {
        //////////            CreateVitalSign(1, 1),
        //////////            CreateVitalSign(2, 2),
        //////////            CreateVitalSign(3, 3)
        //////////        };

        //////////    _repositoryMock
        //////////        .Setup(x =>
        //////////            x.GetAllAsync())
        //////////        .ReturnsAsync(vitalSigns);

        //////////    // Act

        //////////    var result =
        //////////        await _service.GetAllAsync();

        //////////    // Assert

        //////////    var resultList =
        //////////        result.ToList();

        //////////    Assert.Equal(
        //////////        3,
        //////////        resultList.Count);

        //////////    Assert.Equal(
        //////////        1,
        //////////        resultList[0].VitalSignId);

        //////////    Assert.Equal(
        //////////        3,
        //////////        resultList[2].VitalSignId);

        //////////    _repositoryMock.Verify(
        //////////        x =>
        //////////            x.GetAllAsync(),
        //////////        Times.Once);
        //////////}

        // =========================================================
        // Test 5: Create Vital Sign - Success
        // Verifies that a new vital-sign record is created correctly
        // and that the repository receives the expected entity.
        // =========================================================

        //[Fact]
        //public async Task CreateAsync_ReturnsCreatedVitalSign()
        //{
        //    // Arrange

        //    var measuredAt =
        //        new DateTime(
        //            2026,
        //            8,
        //            19,
        //            10,
        //            0,
        //            0);

        //    var dto =
        //        new CreateVitalSignDto
        //        {
        //            PatientId = 3,

        //            RecordedByDoctorName =
        //                "Dr. Ahmad",

        //            HeartRate = 85,

        //            SystolicPressure = 120,

        //            DiastolicPressure = 80,

        //            OxygenSaturation = 98,

        //            Temperature = 36.8m,

        //            MeasuredAt = measuredAt,

        //            Notes =
        //                "Regular measurement."
        //        };

        //    var createdVitalSign =
        //        new VitalSign
        //        {
        //            VitalSignId = 10,

        //            PatientId = 3,

        //            RecordedByDoctorName =
        //                "Dr. Ahmad",

        //            HeartRate = 85,

        //            SystolicPressure = 120,

        //            DiastolicPressure = 80,

        //            OxygenSaturation = 98,

        //            Temperature = 36.8m,

        //            MeasuredAt = measuredAt,

        //            CreatedAt =
        //                DateTime.UtcNow,

        //            Notes =
        //                "Regular measurement."
        //        };

        //    _repositoryMock
        //        .Setup(x =>
        //            x.AddAsync(
        //                It.IsAny<VitalSign>()))
        //        .ReturnsAsync(
        //            createdVitalSign);

        //    // Act

        //    var result =
        //        await _service.CreateAsync(dto);

        //    // Assert

        //    Assert.NotNull(result);

        //    Assert.Equal(
        //        10,
        //        result.VitalSignId);

        //    Assert.Equal(
        //        3,
        //        result.PatientId);

        //    Assert.Equal(
        //        85,
        //        result.HeartRate);

        //    Assert.Equal(
        //        120,
        //        result.SystolicPressure);

        //    Assert.Equal(
        //        80,
        //        result.DiastolicPressure);

        //    Assert.Equal(
        //        98m,
        //        result.OxygenSaturation);

        //    Assert.Equal(
        //        36.8m,
        //        result.Temperature);

        //    _repositoryMock.Verify(
        //        x =>
        //            x.AddAsync(
        //                It.Is<VitalSign>(v =>
        //                    v.PatientId == 3 &&
        //                    v.HeartRate == 85 &&
        //                    v.SystolicPressure == 120 &&
        //                    v.DiastolicPressure == 80)),
        //        Times.Once);
        //}

        // =========================================================
        // Test 6: Get My Vital Signs
        // Verifies that the authenticated user's vital-sign records
        // are returned using the correct user ID.
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
        // Test 7: Filter Vital Signs
        // Verifies that filtering parameters are passed correctly
        // to the repository and filtered records are returned.
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
        // Test 8: Update Vital Sign - Success
        // Verifies that an existing vital-sign record is updated
        // correctly with the supplied values.
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
        // Test 9: Update Vital Sign - Not Found
        // Verifies that update returns false when the requested
        // vital-sign record does not exist and no update is performed.
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
        // Test 10: Delete Vital Sign - Success
        // Verifies that an existing vital-sign record is deleted
        // successfully and that the repository delete method is called.
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
        // Test 11: Delete Vital Sign - Not Found
        // Verifies that delete returns false when the requested
        // vital-sign record does not exist.
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
        // Test 12: Compare Latest Two Vital Signs - Success
        // Verifies comparison between the latest and previous
        // vital-sign measurements.
        // Checks calculated changes and health status for each metric.
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
        // Test 13: Compare Latest Two - Insufficient Data
        // Verifies that comparison returns null when fewer than
        // two vital-sign records are available.
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
        // Test 14: Compare Vital Signs By Dates - Success
        // Verifies comparison between vital-sign records from two
        // selected dates and checks the calculated changes.
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
                .ReturnsAsync(
                    firstVitalSign);

            _repositoryMock
                .Setup(x =>
                    x.GetLatestByUserIdAndDateAsync(
                        3,
                        secondDate))
                .ReturnsAsync(
                    secondVitalSign);

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
        // Test 15: Compare Vital Signs By Dates - Missing Record
        // Verifies that comparison returns null when one of the
        // selected dates does not contain a vital-sign record.
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
                .ReturnsAsync(
                    firstVitalSign);

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
        // Helper Method
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
// =========================================================
// Test 4: Get All Vital Signs - Valid Query
// Verifies that GetAllAsync returns paginated vital-sign
// records correctly and maps the entities to DTOs.
// =========================================================

[Fact]
public async Task GetAllAsync_ValidQuery_ReturnsPaginatedData()
        {
            // Arrange

            var query = new VitalSignQueryDto
            {
                Page = 1,
                PageSize = 10,
                Gender = "Female"
            };

            var vitalSigns =
                new List<VitalSign>
                {
            CreateVitalSign(
                id: 1,
                patientId: 1),

            CreateVitalSign(
                id: 2,
                patientId: 2)
                };

            _repositoryMock
                .Setup(x =>
                    x.GetAllAsync(
                        It.IsAny<VitalSignQueryDto>()))
                .ReturnsAsync(
                    (vitalSigns.AsEnumerable(), 2));

            try
            {
                // Act

                var result =
                    await _service.GetAllAsync(query);

                // Assert 

                Assert.NotNull(result);

                Assert.Equal(
                    2,
                    result.Data.Count());

                Assert.Equal(
                    2,
                    result.TotalCount);

                Assert.Equal(
                    1,
                    result.TotalPages);

                Assert.Equal(
                    1,
                    result.Page);

                Assert.Equal(
                    10,
                    result.PageSize);

                Assert.Equal(
                    1,
                    result.Data.First().VitalSignId);

                Assert.Equal(
                    80,
                    result.Data.First().HeartRate);

                _repositoryMock.Verify(
                    x =>
                        x.GetAllAsync(
                            It.IsAny<VitalSignQueryDto>()),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "GetAllAsync_ValidQuery_ReturnsPaginatedData " +
                    "failed unexpectedly.",
                    ex);
            }
        }


        // =========================================================
        // Test 4.1: Get All Vital Signs - Page Less Than One
        // Verifies that Page is changed to 1 when an invalid
        // page number less than 1 is provided.
        // =========================================================

        [Fact]
        public async Task GetAllAsync_PageLessThanOne_SetsPageToOne()
        {
            // Arrange

            var query = new VitalSignQueryDto
            {
                Page = 0,
                PageSize = 10
            };

            VitalSignQueryDto? capturedQuery = null;

            _repositoryMock
                .Setup(x =>
                    x.GetAllAsync(
                        It.IsAny<VitalSignQueryDto>()))
                .Callback<VitalSignQueryDto>(
                    q => capturedQuery = q)
                .ReturnsAsync(
                    (Enumerable.Empty<VitalSign>(), 0));

            try
            {
                // Act

                var result =
                    await _service.GetAllAsync(query);

                // Assert

                Assert.Equal(
                    1,
                    result.Page);

                Assert.NotNull(capturedQuery);

                Assert.Equal(
                    1,
                    capturedQuery!.Page);

                _repositoryMock.Verify(
                    x =>
                        x.GetAllAsync(
                            It.IsAny<VitalSignQueryDto>()),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "GetAllAsync_PageLessThanOne_SetsPageToOne " +
                    "failed unexpectedly.",
                    ex);
            }
        }


        // =========================================================
        // Test 4.2: Get All Vital Signs - Page Size Less Than One
        // Verifies that PageSize is changed to the default value
        // of 10 when an invalid PageSize is provided.
        // =========================================================

        [Fact]
        public async Task GetAllAsync_PageSizeLessThanOne_SetsDefaultPageSize()
        {
            // Arrange

            var query = new VitalSignQueryDto
            {
                Page = 1,
                PageSize = 0
            };

            VitalSignQueryDto? capturedQuery = null;

            _repositoryMock
                .Setup(x =>
                    x.GetAllAsync(
                        It.IsAny<VitalSignQueryDto>()))
                .Callback<VitalSignQueryDto>(
                    q => capturedQuery = q)
                .ReturnsAsync(
                    (Enumerable.Empty<VitalSign>(), 0));

            try
            {
                // Act

                var result =
                    await _service.GetAllAsync(query);

                // Assert

                Assert.Equal(
                    10,
                    result.PageSize);

                Assert.NotNull(capturedQuery);

                Assert.Equal(
                    10,
                    capturedQuery!.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "GetAllAsync_PageSizeLessThanOne_" +
                    "SetsDefaultPageSize failed unexpectedly.",
                    ex);
            }
        }


        // =========================================================
        // Test 4.3: Get All Vital Signs - Page Size Greater Than 100
        // Verifies that PageSize is limited to 100 when the provided
        // PageSize is greater than the maximum allowed value.
        // =========================================================

        [Fact]
        public async Task GetAllAsync_PageSizeGreaterThan100_LimitsPageSizeTo100()
        {
            // Arrange

            var query = new VitalSignQueryDto
            {
                Page = 1,
                PageSize = 150
            };

            VitalSignQueryDto? capturedQuery = null;

            _repositoryMock
                .Setup(x =>
                    x.GetAllAsync(
                        It.IsAny<VitalSignQueryDto>()))
                .Callback<VitalSignQueryDto>(
                    q => capturedQuery = q)
                .ReturnsAsync(
                    (Enumerable.Empty<VitalSign>(), 0));

            try
            {
                // Act

                var result =
                    await _service.GetAllAsync(query);

                // Assert

                Assert.Equal(
                    100,
                    result.PageSize);

                Assert.NotNull(capturedQuery);

                Assert.Equal(
                    100,
                    capturedQuery!.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "GetAllAsync_PageSizeGreaterThan100_" +
                    "LimitsPageSizeTo100 failed unexpectedly.",
                    ex);
            }
        }


        // =========================================================
        // Test 4.4: Get All Vital Signs - Invalid Gender
        // Verifies that ArgumentException is thrown when an invalid
        // gender value is provided.
        // =========================================================

        [Fact]
        public async Task GetAllAsync_InvalidGender_ThrowsArgumentException()
        {
            // Arrange

            var query = new VitalSignQueryDto
            {
                Page = 1,
                PageSize = 10,
                Gender = "Unknown"
            };

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<ArgumentException>(
                    () =>
                        _service.GetAllAsync(query));

            // Verify exception message.

            Assert.Equal(
                "Invalid gender. Allowed values are Male or Female.",
                exception.Message);

            // Repository must not be called because
            // validation happens before repository access.

            _repositoryMock.Verify(
                x =>
                    x.GetAllAsync(
                        It.IsAny<VitalSignQueryDto>()),
                Times.Never);
        }


        // =========================================================
        // Test 4.5: Get All Vital Signs - No Data
        // Verifies that an empty result is returned when the
        // repository contains no matching vital-sign records.
        // =========================================================

        [Fact]
        public async Task GetAllAsync_NoData_ReturnsEmptyResult()
        {
            // Arrange

            var query = new VitalSignQueryDto
            {
                Page = 1,
                PageSize = 10
            };

            _repositoryMock
                .Setup(x =>
                    x.GetAllAsync(
                        It.IsAny<VitalSignQueryDto>()))
                .ReturnsAsync(
                    (Enumerable.Empty<VitalSign>(), 0));

            try
            {
                // Act

                var result =
                    await _service.GetAllAsync(query);

                // Assert

                Assert.NotNull(result);

                Assert.Empty(result.Data);

                Assert.Equal(
                    0,
                    result.TotalCount);

                Assert.Equal(
                    0,
                    result.TotalPages);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "GetAllAsync_NoData_ReturnsEmptyResult " +
                    "failed unexpectedly.",
                    ex);
            }
        }


        // =========================================================
        // Test 4.6: Get All Vital Signs - Total Pages Calculation
        // Verifies that TotalPages is calculated correctly when
        // the total number of records is not evenly divisible
        // by PageSize.
        // Example: 25 records / 10 per page = 3 pages.
        // =========================================================

        [Fact]
        public async Task GetAllAsync_CalculatesTotalPagesCorrectly()
        {
            // Arrange

            var query = new VitalSignQueryDto
            {
                Page = 1,
                PageSize = 10
            };

            var vitalSigns =
                new List<VitalSign>
                {
            CreateVitalSign(1, 1)
                };

            _repositoryMock
                .Setup(x =>
                    x.GetAllAsync(
                        It.IsAny<VitalSignQueryDto>()))
                .ReturnsAsync(
                    (vitalSigns.AsEnumerable(), 25));

            try
            {
                // Act

                var result =
                    await _service.GetAllAsync(query);

                // Assert

                Assert.Equal(
                    25,
                    result.TotalCount);

                Assert.Equal(
                    3,
                    result.TotalPages);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "GetAllAsync_CalculatesTotalPagesCorrectly " +
                    "failed unexpectedly.",
                    ex);
            }
        }


    }
}