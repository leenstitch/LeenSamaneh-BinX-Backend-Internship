namespace APIProject
{
    public class UnitTest1
    {
        
        [Fact]
        public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Add(5, 3);

            // Assert
            Assert.Equal(8, result);
        }

        [Fact]
        public void Add_PositiveAndNegativeNumber_ReturnsCorrectSum()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Add(5, -3);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Add_TwoNegativeNumbers_ReturnsCorrectSum()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Add(-5, -3);

            // Assert
            Assert.Equal(-8, result);
        }

        [Theory]
        [InlineData(5, 3, 8)]
        [InlineData(5, -3, 2)]
        [InlineData(-5, -3, -8)]
        public void Add_ReturnsCorrectSum(int a, int b, int expected)
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
