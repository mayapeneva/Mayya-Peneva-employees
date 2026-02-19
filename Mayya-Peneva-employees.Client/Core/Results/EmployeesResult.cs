using Mayya_Peneva_employees.Client.Models.ViewModels;

namespace Mayya_Peneva_employees.Client.Core.Results
{
    public class EmployeesResult : BaseAppResult
    {
        public IEnumerable<EmployeesViewModel> EmployeesPerProject { get; set; } = [];
    }
}
