using Mayya_Peneva_employees.Client.Entities;
using Mayya_Peneva_employees.Client.ViewModels;

namespace Mayya_Peneva_employees.Client.Services
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeesViewModel> GetPairEmployeesWorkedLongest(IEnumerable<Employee> employees);
    }
}
