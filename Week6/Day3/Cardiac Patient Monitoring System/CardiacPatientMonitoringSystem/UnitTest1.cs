using Cardiac_Patient_Monitoring_System.Services;
using CardiacPatientMonitoringSystem;

namespace Cardiac_Patient_Monitoring_System.Tests
{
    public class CalculatorServiceTests
    {
        // Fact: Tests one specific scenario with fixed input values.
        [Fact]
        public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
        {
            // Arrange: Create the calculator and prepare the input values.
            var calculator = new CalculatorService();
            int a = 10;
            int b = 5;

            // Act: Call the method that we want to test.
            var result = calculator.Add(a, b);

            // Assert: Verify that the actual result matches the expected result.
            Assert.Equal(15, result);
        }


        // Fact: Tests adding a positive number and a negative number.
        [Fact]
        public void Add_PositiveAndNegativeNumber_ReturnsCorrectSum()
        {
            // Arrange: Create the calculator and prepare the input values.
            var calculator = new CalculatorService();
            int a = 10;
            int b = -5;

            // Act: Call the Add method.
            var result = calculator.Add(a, b);

            // Assert: Verify that the result is correct.
            Assert.Equal(5, result);
        }


        // Fact: Tests adding two negative numbers.
        [Fact]
        public void Add_TwoNegativeNumbers_ReturnsCorrectSum()
        {
            // Arrange: Create the calculator and prepare the input values.
            var calculator = new CalculatorService();
            int a = -10;
            int b = -5;

            // Act: Call the Add method.
            var result = calculator.Add(a, b);

            // Assert: Verify that the result is correct.
            Assert.Equal(-15, result);
        }


        // Theory: Tests the same method with multiple sets of input values.
        [Theory]

        // Each InlineData represents one test case.
        [InlineData(10, 5, 15)]
        [InlineData(-3, 3, 0)]
        [InlineData(-10, -5, -15)]
        public void Add_TwoNumbers_ReturnsCorrectSum(
            int a,
            int b,
            int expected)
        {
            // Arrange: Create the calculator.
            var calculator = new CalculatorService();

            // Act: Call the Add method using the provided test data.
            var result = calculator.Add(a, b);

            // Assert: Compare the actual result with the expected result.
            Assert.Equal(expected, result);
        }
    }
}