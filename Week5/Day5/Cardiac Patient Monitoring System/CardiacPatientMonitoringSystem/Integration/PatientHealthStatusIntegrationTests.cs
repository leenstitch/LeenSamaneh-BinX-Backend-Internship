using System.Net;
using System.Net.Http.Json;
using CardiacPatientMonitoringSystem.Integration;
using Xunit;

namespace CardiacPatientMonitoringSystem.Tests.Integration
{
    // ================================================================
    // Patient Health Status Integration Tests
    // ================================================================
    //
    // These tests verify the complete API request flow:
    //
    // HTTP Request
    //      ↓
    // PatientsController
    //      ↓
    // PatientService
    //      ↓
    // PatientRepository
    //      ↓
    // In-Memory Test Database
    //      ↓
    // HTTP Response
    //
    // Unlike unit tests, these tests use the real application pipeline
    // and do not mock the PatientService or PatientRepository.
    //
    // The test environment uses:
    // - CustomWebApplicationFactory
    // - In-Memory database
    // - Test authentication handler
    //
    // ================================================================

    public class PatientHealthStatusIntegrationTests
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PatientHealthStatusIntegrationTests(
            CustomWebApplicationFactory factory)
        {
            _client =
                factory.CreateClient();
        }

        // =========================================================
        // Test 1: Abnormal Vital Signs
        //
        // Verifies that an authenticated patient with abnormal
        // vital signs receives:
        // - HTTP 200 OK
        // - "Needs Attention" status
        // - The expected health alerts
        // =========================================================

        [Fact]
        public async Task GetMyHealthStatus_ReturnsNeedsAttention_WhenVitalSignsAreAbnormal()
        {
            // Arrange

            var request =
                new HttpRequestMessage(
                    HttpMethod.Get,
                    "/api/v1/PatientsController/me/health-status");

            request.Headers.Add(
                "X-Test-UserId",
                "1");

            request.Headers.Add(
                "X-Test-Role",
                "Patient");

            // Act

            var response =
                await _client.SendAsync(request);

            // Assert

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var result =
                await response.Content
                    .ReadFromJsonAsync<
                        PatientHealthStatusTestResponse>();

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
        // Test 2: No Vital Signs
        //
        // Verifies that an authenticated patient with no
        // vital-sign records receives:
        // - HTTP 200 OK
        // - "No Data" status
        // - No alerts
        // - No latest measurement date
        // =========================================================

        [Fact]
        public async Task GetMyHealthStatus_ReturnsNoData_WhenPatientHasNoVitalSigns()
        {
            // Arrange

            var request =
                new HttpRequestMessage(
                    HttpMethod.Get,
                    "/api/v1/PatientsController/me/health-status");

            request.Headers.Add(
                "X-Test-UserId",
                "2");

            request.Headers.Add(
                "X-Test-Role",
                "Patient");

            // Act

            var response =
                await _client.SendAsync(request);

            // Assert

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var result =
                await response.Content
                    .ReadFromJsonAsync<
                        PatientHealthStatusTestResponse>();

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
        // Test Response Model
        //
        // Used only inside the integration tests to deserialize
        // the API response.
        // =========================================================

        private class PatientHealthStatusTestResponse
        {
            public string Status { get; set; }
                = string.Empty;

            public List<string> Alerts { get; set; }
                = new();

            public DateTime? LatestMeasuredAt { get; set; }
        }
    }
}