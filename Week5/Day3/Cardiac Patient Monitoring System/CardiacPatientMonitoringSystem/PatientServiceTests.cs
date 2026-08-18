// This file contains unit tests for the PatientService class.
// It demonstrates how to use xUnit and Moq to test service methods
// without using the real database or repository implementation.
// The tests cover successful operations, missing patients,
// repository updates, and repository exceptions.

using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Services;
using Moq;

namespace CardiacPatientMonitoringSystem
{
    public class PatientServiceTests
    {
        // Tests that the service successfully returns the patient's profile
        // when a patient exists for the specified user ID.
        [Fact]
        public async Task GetMyProfileAsync_PatientExists_ReturnsPatientProfile()
        {
            // Arrange: Create a mock repository and prepare test patient data.
            var mockRepository = new Mock<IPatientRepository>();

            var patient = new Patient
            {
                PatientId = 1,
                UserId = 10,
                FirstName = "Leen",
                LastName = "Samaneh",
                PatientGender = Patient.Gender.Female
            };

            // Configure the mock repository to return the test patient
            // when GetByUserIdAsync is called with user ID 10.
            mockRepository
                .Setup(r => r.GetByUserIdAsync(10))
                .ReturnsAsync(patient);

            // Create the service and inject the mocked repository.
            var service = new PatientService(mockRepository.Object);

            // Act: Call the method being tested.
            var result = await service.GetMyProfileAsync(10);

            // Assert: Verify that a profile was returned
            // and that the returned data matches the patient data.
            Assert.NotNull(result);
            Assert.Equal("Leen", result.FirstName);
            Assert.Equal("Samaneh", result.LastName);
        }


        // Tests that the service returns null when no patient
        // exists for the specified user ID.
        [Fact]
        public async Task GetMyProfileAsync_PatientDoesNotExist_ReturnsNull()
        {
            // Arrange: Create a mock repository.
            var mockRepository = new Mock<IPatientRepository>();

            // Configure the mock repository to return null
            // when the patient does not exist.
            mockRepository
                .Setup(r => r.GetByUserIdAsync(10))
                .ReturnsAsync((Patient?)null);

            // Create the service using the mocked repository.
            var service = new PatientService(mockRepository.Object);

            // Act: Call the method being tested.
            var result = await service.GetMyProfileAsync(10);

            // Assert: Verify that the service returns null
            // when the patient cannot be found.
            Assert.Null(result);
        }


        // Tests that the service successfully updates an existing patient's profile.
        [Fact]
        public async Task UpdateMyProfileAsync_PatientExists_UpdatesPatient()
        {
            // Arrange: Create a mock repository and an existing patient.
            var mockRepository = new Mock<IPatientRepository>();

            var patient = new Patient
            {
                PatientId = 1,
                UserId = 10,
                FirstName = "Leen",
                LastName = "Samaneh",
                PatientGender = Patient.Gender.Female,
                PrimaryPhone = "0599999999"
            };

            // Configure the mock repository to return the existing patient
            // when searching by the user's ID.
            mockRepository
                .Setup(r => r.GetByUserIdAsync(10))
                .ReturnsAsync(patient);

            // Configure the mock repository to allow the patient update operation
            // without accessing a real database.
            mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            // Configure the mock repository to allow SaveChangesAsync
            // to complete successfully without using a real database.
            mockRepository
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Arrange the new profile information that will be used for the update.
            var dto = new UpdatePatientDto
            {
                FirstName = "Leen Updated",
                LastName = "Samaneh",
                PatientGender = Patient.Gender.Female,
                PrimaryPhone = "0566666666"
            };

            // Create the service using the mocked repository.
            var service = new PatientService(mockRepository.Object);

            // Act: Call the update method with the user ID and new profile data.
            var result = await service.UpdateMyProfileAsync(10, dto);

            // Assert: Verify that the returned profile contains the updated values.
            Assert.NotNull(result);
            Assert.Equal("Leen Updated", result.FirstName);
            Assert.Equal("0566666666", result.PrimaryPhone);

            // Verify that the repository's UpdateAsync method
            // was called exactly once with the expected patient.
            mockRepository.Verify(
                r => r.UpdateAsync(patient),
                Times.Once);

            // Verify that SaveChangesAsync was called exactly once
            // to save the changes.
            mockRepository.Verify(
                r => r.SaveChangesAsync(),
                Times.Once);
        }


        // Tests that the service correctly propagates an exception
        // when the repository fails during the update operation.
        [Fact]
        public async Task UpdateMyProfileAsync_RepositoryThrowsException_ThrowsException()
        {
            // Arrange: Create a mock repository and an existing patient.
            var mockRepository = new Mock<IPatientRepository>();

            var patient = new Patient
            {
                PatientId = 1,
                UserId = 10,
                FirstName = "Leen",
                LastName = "Samaneh",
                PatientGender = Patient.Gender.Female
            };

            // Configure the mock repository to return the existing patient.
            mockRepository
                .Setup(r => r.GetByUserIdAsync(10))
                .ReturnsAsync(patient);

            // Configure the repository to throw an exception
            // when UpdateAsync is called.
            mockRepository
                .Setup(r => r.UpdateAsync(patient))
                .ThrowsAsync(new InvalidOperationException());

            // Prepare the data that will be used for the update.
            var dto = new UpdatePatientDto
            {
                FirstName = "Updated",
                LastName = "Samaneh",
                PatientGender = Patient.Gender.Female
            };

            // Create the service using the mocked repository.
            var service = new PatientService(mockRepository.Object);

            // Act & Assert: Call the update method and verify that
            // the expected exception is thrown.
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.UpdateMyProfileAsync(10, dto));
        }
    }
}