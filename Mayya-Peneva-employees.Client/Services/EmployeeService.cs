using Mayya_Peneva_employees.Client.Entities;
using Mayya_Peneva_employees.Client.ViewModels;

namespace Mayya_Peneva_employees.Client.Services
{
    public class EmployeeService : IEmployeeService
    {
        public Task<IEnumerable<EmployeesViewModel>> GetPairEmployeesWorkedLongest(IEnumerable<Employee> employees)
        {
            return null;
        }
    }
}
