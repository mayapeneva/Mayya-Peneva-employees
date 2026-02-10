using Mayya_Peneva_employees.Client.ViewModels;

namespace Mayya_Peneva_employees.Client.Core.Results
{
    public class EmploeeysResult : BaseAppResult
    {
        public IEnumerable<KeyValuePair<int, int>> EmployeeIdsWorkedTogether { get; set; } = [];
        public IEnumerable<EmployeesViewModel> EmployeesPerProject { get; set; } = [];
    }
}
