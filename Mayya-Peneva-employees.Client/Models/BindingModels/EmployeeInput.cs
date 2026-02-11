using CsvHelper.Configuration.Attributes;
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
    }
}
