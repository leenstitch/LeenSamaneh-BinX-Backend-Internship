using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Services;
using Moq;
using Xunit;

namespace CardiacPatientMonitoringSystem.XunitMoq
{
    public class MedicalTimelineServiceTests
    {
        private readonly Mock<IAllergyRepository>
            _allergyRepositoryMock;

        private readonly Mock<IFamilyMedicalHistoryRepository>
            _familyHistoryRepositoryMock;

        private readonly Mock<IVitalSignRepository>
            _vitalSignRepositoryMock;

        private readonly Mock<IMedicationRepository>
            _medicationRepositoryMock;

        private readonly Mock<IDiagnosisRepository>
            _diagnosisRepositoryMock;

        private readonly Mock<IAppointmentRepository>
            _appointmentRepositoryMock;

        private readonly Mock<ILabResultRepository>
            _labResultRepositoryMock;

        private readonly Mock<IHospitalizationRepository>
            _hospitalizationRepositoryMock;

        private readonly MedicalTimelineService _service;


        public MedicalTimelineServiceTests()
        {
            _allergyRepositoryMock =
                new Mock<IAllergyRepository>();

            _familyHistoryRepositoryMock =
                new Mock<IFamilyMedicalHistoryRepository>();

            _vitalSignRepositoryMock =
                new Mock<IVitalSignRepository>();

            _medicationRepositoryMock =
                new Mock<IMedicationRepository>();

            _diagnosisRepositoryMock =
                new Mock<IDiagnosisRepository>();

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _labResultRepositoryMock =
                new Mock<ILabResultRepository>();

            _hospitalizationRepositoryMock =
                new Mock<IHospitalizationRepository>();


            _service =
                new MedicalTimelineService(
                    _allergyRepositoryMock.Object,
                    _familyHistoryRepositoryMock.Object,
                    _vitalSignRepositoryMock.Object,
                    _medicationRepositoryMock.Object,
                    _diagnosisRepositoryMock.Object,
                    _labResultRepositoryMock.Object,
                    _hospitalizationRepositoryMock.Object,
                    _appointmentRepositoryMock.Object);
        }

        // =========================================================
        // Test 1: Get Patient Medical Timeline - Success
        //
        // Verifies that the service collects medical information
        // from different repositories and combines it into one
        // unified medical timeline.
        // =========================================================

        [Fact]
        public async Task GetPatientMedicalTimelineAsync_ReturnsTimeline_WhenMedicalDataExists()
        {
            // Arrange

            int patientId = 5;

            var allergy = new Allergy
            {
                AllergyId = 1,
                PatientId = patientId,
                Name = "Penicillin",
                Reaction = "Rash",
                CreatedAt = new DateTime(2026, 8, 10)
            };

            var familyHistory = new FamilyMedicalHistory
            {
                FamilyHistoryId = 2,
                PatientId = patientId,
                Relationship = "Father",
                Condition = "Heart Disease",
                CreatedAt = new DateTime(2026, 8, 11)
            };

            var vitalSign = new VitalSign
            {
                VitalSignId = 3,
                PatientId = patientId,
                HeartRate = 80,
                SystolicPressure = 120,
                DiastolicPressure = 80,
                OxygenSaturation = 98,
                MeasuredAt = new DateTime(2026, 8, 12)
            };

            var medication = new Medication
            {
                MedicationId = 4,
                PatientId = patientId,
                Name = "Aspirin",
                Dosage = "100mg",
                Frequency = "Once daily",
                StartDate = new DateTime(2026, 8, 13)
            };

            var diagnosis = new Diagnosis
            {
                DiagnosisId = 5,
                PatientId = patientId,
                DiagnosisName = "Hypertension",
                DiagnosedAt = new DateTime(2026, 8, 14),
                Notes = "High blood pressure"
            };

            var appointment = new Appointment
            {
                AppointmentId = 6,
                PatientId = patientId,
                AppointmentDate = new DateTime(2026, 8, 15),
                Reason = "Follow-up",
                Notes = "Regular check-up"
            };


            _allergyRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Allergy>
                    {
                        allergy
                    });

            _familyHistoryRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<FamilyMedicalHistory>
                    {
                        familyHistory
                    });

            _vitalSignRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<VitalSign>
                    {
                        vitalSign
                    });

            _medicationRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Medication>
                    {
                        medication
                    });

            _diagnosisRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Diagnosis>
                    {
                        diagnosis
                    });

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Appointment>
                    {
                        appointment
                    });


            // Act

            var result =
                await _service
                    .GetPatientMedicalTimelineAsync(
                        patientId,
                        1,
                        10);


            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                patientId,
                result.PatientId);

            Assert.Equal(
                6,
                result.TotalCount);

            Assert.Equal(
                6,
                result.Items.Count);

            Assert.Contains(
                result.Items,
                x => x.EventType == "Allergy");

            Assert.Contains(
                result.Items,
                x => x.EventType == "Family Medical History");

            Assert.Contains(
                result.Items,
                x => x.EventType == "Vital Sign");

            Assert.Contains(
                result.Items,
                x => x.EventType == "Medication");

            Assert.Contains(
                result.Items,
                x => x.EventType == "Diagnosis");

            Assert.Contains(
                result.Items,
                x => x.EventType == "Appointment");


            // Verify repositories were called

            _allergyRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(patientId),
                Times.Once);

            _familyHistoryRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(patientId),
                Times.Once);

            _vitalSignRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(patientId),
                Times.Once);

            _medicationRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(patientId),
                Times.Once);

            _diagnosisRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(patientId),
                Times.Once);

            _appointmentRepositoryMock.Verify(
                x =>
                    x.GetByPatientIdAsync(patientId),
                Times.Once);
        }
        // =========================================================
        // Test 2: Timeline Sorting
        //
        // Verifies that timeline items are sorted from newest
        // medical event to oldest medical event.
        // =========================================================

        [Fact]
        public async Task GetPatientMedicalTimelineAsync_SortsTimelineFromNewestToOldest()
        {
            // Arrange

            int patientId = 5;

            var oldAllergy = new Allergy
            {
                AllergyId = 1,
                PatientId = patientId,
                Name = "Dust",
                Reaction = "Sneezing",
                CreatedAt = new DateTime(2026, 8, 1)
            };

            var recentDiagnosis = new Diagnosis
            {
                DiagnosisId = 2,
                PatientId = patientId,
                DiagnosisName = "Hypertension",
                DiagnosedAt = new DateTime(2026, 8, 20),
                Notes = "High blood pressure"
            };


            _allergyRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Allergy>
                    {
                        oldAllergy
                    });

            _familyHistoryRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<FamilyMedicalHistory>());

            _vitalSignRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<VitalSign>());

            _medicationRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Medication>());

            _diagnosisRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Diagnosis>
                    {
                        recentDiagnosis
                    });

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Appointment>());


            // Act

            var result =
                await _service
                    .GetPatientMedicalTimelineAsync(
                        patientId,
                        1,
                        10);


            // Assert

            Assert.Equal(
                "Diagnosis",
                result.Items[0].EventType);

            Assert.Equal(
                "Allergy",
                result.Items[1].EventType);
        }
        // =========================================================
        // Test 3: Pagination - First Page
        //
        // Verifies that the service returns only the requested
        // number of timeline items for the first page.
        // =========================================================

        [Fact]
        public async Task GetPatientMedicalTimelineAsync_ReturnsCorrectFirstPage()
        {
            // Arrange

            int patientId = 5;

            _allergyRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Allergy>
                    {
                        new Allergy
                        {
                            AllergyId = 1,
                            PatientId = patientId,
                            Name = "Dust",
                            CreatedAt = new DateTime(2026, 8, 1)
                        },
                        new Allergy
                        {
                            AllergyId = 2,
                            PatientId = patientId,
                            Name = "Pollen",
                            CreatedAt = new DateTime(2026, 8, 2)
                        },
                        new Allergy
                        {
                            AllergyId = 3,
                            PatientId = patientId,
                            Name = "Peanuts",
                            CreatedAt = new DateTime(2026, 8, 3)
                        }
                    });

            _familyHistoryRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(new List<FamilyMedicalHistory>());

            _vitalSignRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(new List<VitalSign>());

            _medicationRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(new List<Medication>());

            _diagnosisRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(new List<Diagnosis>());

            _appointmentRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(new List<Appointment>());


            // Act

            var result =
                await _service
                    .GetPatientMedicalTimelineAsync(
                        patientId,
                        1,
                        2);


            // Assert

            Assert.Equal(
                1,
                result.Page);

            Assert.Equal(
                2,
                result.PageSize);

            Assert.Equal(
                3,
                result.TotalCount);

            Assert.Equal(
                2,
                result.TotalPages);

            Assert.Equal(
                2,
                result.Items.Count);
        }

        // =========================================================
        // Test 4: Pagination - Second Page
        //
        // Verifies that the second page returns the remaining
        // timeline records correctly.
        // =========================================================

        [Fact]
        public async Task GetPatientMedicalTimelineAsync_ReturnsCorrectSecondPage()
        {
            // Arrange

            int patientId = 5;

            _allergyRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Allergy>
                    {
                        new Allergy
                        {
                            AllergyId = 1,
                            PatientId = patientId,
                            Name = "Dust",
                            CreatedAt = new DateTime(2026, 8, 1)
                        },
                        new Allergy
                        {
                            AllergyId = 2,
                            PatientId = patientId,
                            Name = "Pollen",
                            CreatedAt = new DateTime(2026, 8, 2)
                        },
                        new Allergy
                        {
                            AllergyId = 3,
                            PatientId = patientId,
                            Name = "Peanuts",
                            CreatedAt = new DateTime(2026, 8, 3)
                        }
                    });

            _familyHistoryRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(new List<FamilyMedicalHistory>());

            _vitalSignRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(new List<VitalSign>());

            _medicationRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(new List<Medication>());

            _diagnosisRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(new List<Diagnosis>());

            _appointmentRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(new List<Appointment>());


            // Act

            var result =
                await _service
                    .GetPatientMedicalTimelineAsync(
                        patientId,
                        2,
                        2);


            // Assert

            Assert.Equal(
                2,
                result.Page);

            Assert.Equal(
                2,
                result.PageSize);

            Assert.Equal(
                3,
                result.TotalCount);

            Assert.Equal(
                2,
                result.TotalPages);

            Assert.Single(
                result.Items);
        }
        // =========================================================
        // Test 5: Invalid Page
        //
        // Verifies that a page number less than 1 is automatically
        // changed to page 1.
        // =========================================================

        [Fact]
        public async Task GetPatientMedicalTimelineAsync_UsesPageOne_WhenPageIsLessThanOne()
        {
            // Arrange

            int patientId = 5;

            SetupEmptyRepositories(patientId);


            // Act

            var result =
                await _service
                    .GetPatientMedicalTimelineAsync(
                        patientId,
                        0,
                        10);


            // Assert

            Assert.Equal(
                1,
                result.Page);
        }
        // =========================================================
        // Test 6: Invalid Page Size
        //
        // Verifies that a page size less than 1 is automatically
        // changed to the default value of 10.
        // =========================================================

        [Fact]
        public async Task GetPatientMedicalTimelineAsync_UsesDefaultPageSize_WhenPageSizeIsLessThanOne()
        {
            // Arrange

            int patientId = 5;

            SetupEmptyRepositories(patientId);


            // Act

            var result =
                await _service
                    .GetPatientMedicalTimelineAsync(
                        patientId,
                        1,
                        0);


            // Assert

            Assert.Equal(
                10,
                result.PageSize);
        }

        // =========================================================
        // Test 7: No Medical Data
        //
        // Verifies that an empty timeline is returned when the
        // patient has no medical records.
        // =========================================================

        [Fact]
        public async Task GetPatientMedicalTimelineAsync_ReturnsEmptyTimeline_WhenNoMedicalDataExists()
        {
            // Arrange

            int patientId = 100;

            SetupEmptyRepositories(patientId);


            // Act

            var result =
                await _service
                    .GetPatientMedicalTimelineAsync(
                        patientId,
                        1,
                        10);


            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                patientId,
                result.PatientId);

            Assert.Equal(
                0,
                result.TotalCount);

            Assert.Equal(
                0,
                result.TotalPages);

            Assert.Empty(
                result.Items);
        }
        // =========================================================
        // Test 8: Repository Exception
        //
        // Verifies that an exception thrown by the repository
        // is not silently ignored by the service.
        // The exception should be propagated to the caller.
        // =========================================================

        [Fact]
        public async Task GetPatientMedicalTimelineAsync_ThrowsException_WhenRepositoryFails()
        {
            // Arrange

            int patientId = 5;

            _allergyRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ThrowsAsync(
                    new InvalidOperationException(
                        "Database error."));


            // Act + Assert

            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(
                    async () =>
                        await _service
                            .GetPatientMedicalTimelineAsync(
                                patientId,
                                1,
                                10));


            Assert.Equal(
                "Database error.",
                exception.Message);
        }



        // =========================================================
        // Helper Method
        //
        // Configures all currently used repositories to return
        // empty collections for the specified patient.
        // =========================================================

        private void SetupEmptyRepositories(int patientId)
        {
            _allergyRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Allergy>());

            _familyHistoryRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<FamilyMedicalHistory>());

            _vitalSignRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<VitalSign>());

            _medicationRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Medication>());

            _diagnosisRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Diagnosis>());

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByPatientIdAsync(patientId))
                .ReturnsAsync(
                    new List<Appointment>());
        }
    }
}
    
