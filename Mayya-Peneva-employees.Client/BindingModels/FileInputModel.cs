using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Mayya_Peneva_employees.Client.BindingModels
{
    public class FileInputModel
    {
        [Required(ErrorMessage = "Please select a CSV file.")]
        public IBrowserFile? CsvFile { get; set; }
    }
}
