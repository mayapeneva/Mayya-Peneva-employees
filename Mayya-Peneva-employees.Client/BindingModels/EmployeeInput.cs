using CsvHelper.Configuration.Attributes;
using Mayya_Peneva_employees.Client.Entities;
using System.ComponentModel.DataAnnotations;

namespace Mayya_Peneva_employees.Client.BindingModels
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

        public Employee? ConvertToEmployee()
        {
            var isDateFromParsed = DateOnly.TryParse(this.DateFrom, out var dateFrom);
            var isDateToParsed = DateOnly.TryParse(this.DateTo ?? DateTime.Now.ToShortDateString(), out var dateTo);
            if (!isDateFromParsed || !isDateToParsed)
            {
                return null;
            }

            return new Employee
            {
                Id = this.Id,
                ProjectId = this.ProjectId,
                DateFrom = dateFrom,
                DateTo = dateTo
            };
        }
    }
}
