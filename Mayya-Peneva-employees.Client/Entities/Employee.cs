using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Mayya_Peneva_employees.Client.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public DateOnly DateFrom { get; set; }

        public DateOnly DateTo { get; set; }
    }
}
