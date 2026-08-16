namespace APIProject
{
    public class UnitTest1
    {
        // Fact is used to test one specific scenario with fixed input values.
        [Fact]
        public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
        {
            // Arrange: Create the object that we want to test.
            var calculator = new Calculator();

            // Act: Call the Add method with two positive numbers.
            var result = calculator.Add(5, 3);

            // Assert: Verify that the actual result matches the expected result.
            Assert.Equal(8, result);
        }

        // This test checks that the Add method works with a positive and a negative number.
        [Fact]
        public void Add_PositiveAndNegativeNumber_ReturnsCorrectSum()
        {
            // Arrange: Create the Calculator object.
            var calculator = new Calculator();

            // Act: Add a positive number and a negative number.
            var result = calculator.Add(5, -3);

            // Assert: Verify that the result is 2.
            Assert.Equal(2, result);
        }

        // This test checks that the Add method works with two negative numbers.
        [Fact]
        public void Add_TwoNegativeNumbers_ReturnsCorrectSum()
        {
            // Arrange: Create the Calculator object.
            var calculator = new Calculator();

            // Act: Add two negative numbers.
            var result = calculator.Add(-5, -3);

            // Assert: Verify that the result is -8.
            Assert.Equal(-8, result);
        }

        // Theory allows us to test the same method with multiple sets of input data.
        [Theory]

        // Each InlineData represents one test case:
        // (first number, second number, expected result)
        [InlineData(5, 3, 8)]
        [InlineData(5, -3, 2)]
        [InlineData(-5, -3, -8)]
        public void Add_ReturnsCorrectSum(int a, int b, int expected)
        {
            // Arrange: Create the Calculator object.
            var calculator = new Calculator();

            // Act: Call the Add method using the values provided by InlineData.
            var result = calculator.Add(a, b);

            // Assert: Verify that the actual result matches the expected result.
            Assert.Equal(expected, result);
        }
    }
}