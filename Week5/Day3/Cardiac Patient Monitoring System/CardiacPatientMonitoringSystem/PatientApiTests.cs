using System.Net;
using System.Net.Http.Json;
using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;

namespace CardiacPatientMonitoringSystem
{
    public class PatientApiTests
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PatientApiTests(
            CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        // Tests the happy path for the authenticated patient profile endpoint.
        [Fact]
        public async Task GetMyProfile_ReturnsOk_WhenPatientExists()
        {
            // Act
            var response =
                await _client.GetAsync("/api/Patients/me");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var patient =
                await response.Content
                    .ReadFromJsonAsync<PatientResponseDto>();

            Assert.NotNull(patient);
            Assert.Equal("Test", patient.FirstName);
            Assert.Equal("Patient", patient.LastName);
            Assert.Equal("Male", patient.PatientGender);
        }


        // Tests the not-found path when the authenticated user
        // does not have a patient profile.
        [Fact]
        public async Task GetMyProfile_ReturnsNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            _client.DefaultRequestHeaders.Add(
                "X-Test-UserId",
                "999");
               
            // Act
            var response =
                await _client.GetAsync("/api/Patients/me");
        
            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
        [Fact]
        public async Task GetAllPatients_ReturnsOk_WithPatients()
        {
            // Act
            var response =
                await _client.GetAsync("/api/Patients");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var patients =
                await response.Content
                    .ReadFromJsonAsync<List<PatientResponseDto>>();

            Assert.NotNull(patients);
            Assert.NotEmpty(patients);

            var patient = patients.First();

            Assert.Equal(1, patient.PatientId);
            Assert.Equal("Test", patient.FirstName);
            Assert.Equal("Patient", patient.LastName);
        }
        [Fact]
        public async Task GetMyProfile_ReturnsForbidden_WhenUserIsNotPatient()
        {
            // Arrange
            _client.DefaultRequestHeaders.Add(
                "X-Test-Role",
                "Doctor");

            // Act
            var response =
                await _client.GetAsync("/api/Patients/me");

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        [Fact]
        public async Task GetPatientById_ReturnsOk_WhenPatientExists()
        {
            // Act
            var response =
                await _client.GetAsync("/api/Patients/1");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var patient =
                await response.Content
                    .ReadFromJsonAsync<PatientResponseDto>();

            Assert.NotNull(patient);

            Assert.Equal(1, patient.PatientId);
            Assert.Equal("Test", patient.FirstName);
            Assert.Equal("Patient", patient.LastName);
            Assert.Equal("Male", patient.PatientGender);
        }
        [Fact]
        public async Task GetPatientById_ReturnsNotFound_WhenPatientDoesNotExist()
        {
            // Act
            var response =
                await _client.GetAsync("/api/Patients/99999");

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
    }
}