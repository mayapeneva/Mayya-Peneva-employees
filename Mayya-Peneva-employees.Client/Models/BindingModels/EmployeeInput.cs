using CsvHelper.Configuration.Attributes;
using Mayya_Peneva_employees.Client.Core.Entities;
using Mayya_Peneva_employees.Client.Core.Results;
using System.ComponentModel.DataAnnotations;

namespace Mayya_Peneva_employees.Client.Models.BindingModels
{
    public class EmployeeInput
    {
        [Required]
        [Name("EmpId")]
        public int Id { get; set; }

        [Required]
        [Name("ProjectID")]
        public int ProjectId { get; set; }

        [Required]
        public string DateFrom { get; set; }

        public string? DateTo { get; set; }

        public InputResult ConvertToEmployee()
        {
            var result = new InputResult();

            if (this.Id <= 0)
            {
                result.Errors.Add($"Invalid employee Id: {this.Id}. Employee ID must be a positive integer.");
                return result;
            }

            if (this.ProjectId <= 0)
            {
                result.Errors.Add($"Invalid project Id for employee with Id {this.Id}. Project ID must be a positive integer.");
                return result;
            }

            var isDateFromParsed = DateOnly.TryParse(this.DateFrom, out var dateFrom);
            if (!isDateFromParsed)
                result.Errors.Add($"Invalid date format for employee with Id {this.Id}. Please use a valid date format for DateFrom");
            var dateToValue = string.IsNullOrWhiteSpace(this.DateTo) || this.DateTo.Equals("null", StringComparison.OrdinalIgnoreCase) ? DateOnly.MaxValue.ToString() : this.DateTo;
            var isDateToParsed = DateOnly.TryParse(dateToValue, out var dateTo);
            if (!isDateToParsed)
                result.Errors.Add($"Invalid date format for employee with Id {this.Id}. Please use a valid date format for DateTo.");

            if(!result.IsSuccessful())
                return result;

            result.Employee = new Employee
            {
                Id = this.Id,
                ProjectId = this.ProjectId,
                DateFrom = dateFrom,
                DateTo = dateTo
            };

            return result;
        }
    }
}
