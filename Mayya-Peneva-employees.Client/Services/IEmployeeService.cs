using Mayya_Peneva_employees.Client.Entities;
using Mayya_Peneva_employees.Client.ViewModels;

namespace Mayya_Peneva_employees.Client.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeesViewModel>> GetPairEmployeesWorkedLongest(IEnumerable<Employee> employees);
    }
}
