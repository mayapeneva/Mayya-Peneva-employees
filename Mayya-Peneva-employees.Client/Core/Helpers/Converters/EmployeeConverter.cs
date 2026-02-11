using Mayya_Peneva_employees.Client.Core.Entities;
using Mayya_Peneva_employees.Client.Core.Helpers.Parsers;
using Mayya_Peneva_employees.Client.Core.Results;
using Mayya_Peneva_employees.Client.Models.BindingModels;

namespace Mayya_Peneva_employees.Client.Core.Helpers.Converters
{
    public class EmployeeConverter : IEmployeeConverter
    {
        private readonly IDateParser _dateParser;

        public EmployeeConverter(IDateParser dateParser)
        {
            _dateParser = dateParser;
        }

        public InputResult ConvertEmployeeInput(EmployeeInput employeeInput)
        {
            var result = new InputResult();

            if (employeeInput.Id <= 0)
            {
                result.Errors.Add($"Invalid employee Id: {employeeInput.Id}. Employee ID must be a positive integer.");
                return result;
            }

            if (employeeInput.ProjectId <= 0)
            {
                result.Errors.Add($"Invalid project Id for employee with Id {employeeInput.Id}. Project ID must be a positive integer.");
                return result;
            }

            var dateFromResult = _dateParser.TryParseDate(employeeInput.DateFrom, nameof(employeeInput.DateFrom), employeeInput.Id);
            if (!dateFromResult.IsSuccessful())
            {
                foreach (var error in dateFromResult.Errors)
                    result.Errors.Add(error);

                return result;
            }

            var dateToString = string.IsNullOrWhiteSpace(employeeInput.DateTo) || employeeInput.DateTo.Equals("null", StringComparison.OrdinalIgnoreCase)
                ? DateOnly.FromDateTime(DateTime.UtcNow).ToString("dd-MM-yyyy")
                : employeeInput.DateTo;

            var dateToResult = _dateParser.TryParseDate(dateToString, nameof(employeeInput.DateTo), employeeInput.Id);
            if (!dateFromResult.IsSuccessful())
            {
                foreach (var error in dateFromResult.Errors)
                    result.Errors.Add(error);

                return result;
            }

            if (dateFromResult.ParsedDate > dateToResult.ParsedDate)
            {
                result.Errors.Add($"Invalid date range for employee with Id {employeeInput.Id}. DateFrom ({dateFromResult.ParsedDate}) cannot be after DateTo ({dateToResult.ParsedDate}).");
                return result;
            }

            result.Employee = new Employee
            {
                Id = employeeInput.Id,
                ProjectId = employeeInput.ProjectId,
                DateFrom = dateFromResult.ParsedDate,
                DateTo = dateToResult.ParsedDate
            };

            return result;
        }
    }
}
