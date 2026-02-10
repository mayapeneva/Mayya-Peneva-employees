using Mayya_Peneva_employees.Client.Core.Entities;
using Mayya_Peneva_employees.Client.ViewModels;

namespace Mayya_Peneva_employees.Client.Core.Services
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeesViewModel> GetPairEmployeesWorkedLongest(IEnumerable<Employee> employees);
    }
}
