using Mayya_Peneva_employees.Client.Core.Entities;
using Mayya_Peneva_employees.Client.Core.Results;

namespace Mayya_Peneva_employees.Client.Core.Services
{
    public interface IEmployeeService
    {
        EmploeeysResult GetPairEmployeesWorkedLongest(IEnumerable<Employee> employees);
    }
}
