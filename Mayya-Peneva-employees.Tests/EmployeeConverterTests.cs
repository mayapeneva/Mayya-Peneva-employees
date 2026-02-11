using Mayya_Peneva_employees.Client.Core.Helpers.Converters;
using Mayya_Peneva_employees.Client.Core.Helpers.Parsers;
using Mayya_Peneva_employees.Client.Models.BindingModels;

namespace Mayya_Peneva_employees.Tests
{
    public class EmployeeInputConverterTests
    {
        private readonly IEmployeeConverter _converter;

        public EmployeeInputConverterTests()
        {
            var dateParser = new DateParser();
            _converter = new EmployeeConverter(dateParser);
        }

        [Fact]
        public void Convert_ShouldReturnsCorrectResultWhenValidInput()
        {
            // Arrange
            var input = new EmployeeInput { Id = 1, ProjectId = 101, DateFrom = "2020-01-15", DateTo = "2020-12-31" };

            // Act
            var actualResult = _converter.ConvertEmployeeInput(input);

            // Assert
            Assert.NotNull(actualResult);
            Assert.True(actualResult.IsSuccessful());
            Assert.NotNull(actualResult.Employee);
            Assert.Equal(1, actualResult.Employee.Id);
            Assert.Equal(101, actualResult.Employee.ProjectId);
            Assert.Equal(new DateOnly(2020, 1, 15), actualResult.Employee.DateFrom);
            Assert.Equal(new DateOnly(2020, 12, 31), actualResult.Employee.DateTo);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Convert_ShouldReturnErrorWhenInvalidEmployeeId(int invalidId)
        {
            // Arrange
            var input = new EmployeeInput { Id = invalidId, ProjectId = 101, DateFrom = "2020-01-01" };

            // Act
            var actualResult = _converter.ConvertEmployeeInput(input);

            // Assert
            Assert.NotNull(actualResult);
            Assert.False(actualResult.IsSuccessful());
            Assert.NotEmpty(actualResult.Errors.First());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Convert_ShouldReturnErrorWhenInvalidProjectId(int invalidProjectId)
        {
            // Arrange
            var input = new EmployeeInput { Id = 1, ProjectId = invalidProjectId, DateFrom = "2020-01-01" };

            // Act
            var actualResult = _converter.ConvertEmployeeInput(input);

            // Assert
            Assert.NotNull(actualResult);
            Assert.False(actualResult.IsSuccessful());
            Assert.NotEmpty(actualResult.Errors.First());
        }

        [Fact]
        public void Convert_ShouldReturnErrorWhenDateFromAfterDateTo()
        {
            // Arrange
            var input = new EmployeeInput { Id = 1, ProjectId = 101, DateFrom = "2020-12-31", DateTo = "2020-01-01" };

            // Act
            var actualResult = _converter.ConvertEmployeeInput(input);

            // Assert
            Assert.NotNull(actualResult);
            Assert.False(actualResult.IsSuccessful());
            Assert.NotEmpty(actualResult.Errors.First());
        }

        [Theory]
        [InlineData("0-0")]
        [InlineData("0")]
        [InlineData("bla")]
        [InlineData("today")]
        public void Convert_ShouldReturnErrorWhenInvalidDateTo(string? dateTo)
        {
            // Arrange
            var input = new EmployeeInput { Id = 1, ProjectId = 101, DateFrom = "2020-01-01", DateTo = dateTo };

            // Act
            var actualResult = _converter.ConvertEmployeeInput(input);

            // Assert
            Assert.NotNull(actualResult);
            Assert.False(actualResult.IsSuccessful());
            Assert.NotEmpty(actualResult.Errors.First());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("null")]
        public void Convert_ShouldReturnCorrectResultWhenNullDateTo(string? dateTo)
        {
            // Arrange
            var input = new EmployeeInput { Id = 1, ProjectId = 101, DateFrom = "2020-01-01", DateTo = dateTo };

            // Act
            var actualResult = _converter.ConvertEmployeeInput(input);

            // Assert
            Assert.NotNull(actualResult);
            Assert.True(actualResult.IsSuccessful());
            Assert.NotNull(actualResult.Employee);
            Assert.Equal(1, actualResult.Employee.Id);
            Assert.Equal(101, actualResult.Employee.ProjectId);
            Assert.Equal(new DateOnly(2020, 1, 1), actualResult.Employee.DateFrom);
            Assert.Equal(DateOnly.FromDateTime(DateTime.UtcNow).ToString("dd-MM-yyyy"), actualResult.Employee.DateTo.ToString("dd-MM-yyyy"));
        }
    }
}
