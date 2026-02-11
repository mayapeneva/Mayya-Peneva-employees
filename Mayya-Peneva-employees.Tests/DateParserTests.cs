using Mayya_Peneva_employees.Client.Core.Helpers.Parsers;

namespace Mayya_Peneva_employees.Tests
{
    public class DateParserTests
    {
        private const string FieldName = "Date";
        private const int EmployeeId = 1;

        private readonly DateParser _parser;

        public DateParserTests()
        {
            _parser = new DateParser();
        }

        [Theory]
        [InlineData("2020-01-15", 2020, 1, 15)]
        [InlineData("15/01/2020", 2020, 1, 15)]
        [InlineData("01/15/2020", 2020, 1, 15)]
        [InlineData("15.01.2020", 2020, 1, 15)]
        [InlineData("Jan 15, 2020", 2020, 1, 15)]
        [InlineData("15 Jan 2020", 2020, 1, 15)]
        public void TryParse_ShouldReturnCorrectResultWhenValidDates(string dateString, int expectedYear, int expectedMonth, int expectedDay)
        {
            // Arrange
            var expectedDate = new DateOnly(expectedYear, expectedMonth, expectedDay);

            // Act
            var actualResult = _parser.TryParseDate(dateString, FieldName, EmployeeId);

            // Assert
            Assert.NotNull(actualResult);
            Assert.True(actualResult.IsSuccessful());
            Assert.Equal(expectedDate, actualResult.ParsedDate);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("32/13/2020")]
        [InlineData("")]
        [InlineData(null)]
        public void TryParse_ShouldReturnErrorWhenInvalidDates(string dateString)
        {
            // Act
            var actualResult = _parser.TryParseDate(dateString, FieldName, EmployeeId);

            // Assert
            Assert.NotNull(actualResult);
            Assert.False(actualResult.IsSuccessful());
            Assert.NotEmpty(actualResult.Errors);
        }
    }
}
