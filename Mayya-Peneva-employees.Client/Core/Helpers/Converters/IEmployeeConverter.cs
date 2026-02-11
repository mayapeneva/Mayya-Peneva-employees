using Mayya_Peneva_employees.Client.Core.Results;
using Mayya_Peneva_employees.Client.Models.BindingModels;

namespace Mayya_Peneva_employees.Client.Core.Helpers.Converters
{
    public interface IEmployeeConverter
    {
        InputResult ConvertEmployeeInput(EmployeeInput employeeInput);
    }
}
