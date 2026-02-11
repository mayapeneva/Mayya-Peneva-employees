using Mayya_Peneva_employees.Client.Core.Results;
using Microsoft.AspNetCore.Components.Forms;

namespace Mayya_Peneva_employees.Client.Core.Helpers.Validators
{
    public interface IFileValidator
    {
        FileValidationResult Validate(IBrowserFile file);
    }
}
