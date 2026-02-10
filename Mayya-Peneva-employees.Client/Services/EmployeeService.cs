using Mayya_Peneva_employees.Client.Entities;
using Mayya_Peneva_employees.Client.ViewModels;

namespace Mayya_Peneva_employees.Client.Services
{
    public class EmployeeService : IEmployeeService
    {
        public IEnumerable<EmployeesViewModel> GetPairEmployeesWorkedLongest(IEnumerable<Employee> employees)
        {
            var employeeOneId = 0;
            var employeeTwoId = 0;
            var highestDaysWorkedTogether = 0;

            var employeePairs = new List<EmployeesViewModel>();

            var employeesByProject = employees.GroupBy(e => e.ProjectId);
            foreach (var projectGroup in employeesByProject)
            {
                var projectEmployees = projectGroup.ToArray();
                for (int i = 0; i < projectEmployees.Length; i++)
                {
                    for (int j = i + 1; j < projectEmployees.Length; j++)
                    {
                        var employeeOne = projectEmployees[i];
                        var employeeTwo = projectEmployees[j];
                        if (!HasOverlap(employeeOne.DateFrom, employeeOne.DateTo, employeeTwo.DateFrom, employeeTwo.DateTo))
                            continue;


                        var overlapStart = MaxDate(employeeOne.DateFrom, employeeTwo.DateFrom);
                        var overlapEnd = MinDate(employeeOne.DateTo, employeeTwo.DateTo);
                        if (overlapStart <= overlapEnd)
                        {
                            var daysWorked = (overlapEnd.DayNumber - overlapStart.DayNumber) + 1;
                            if (daysWorked > highestDaysWorkedTogether)
                            {
                                highestDaysWorkedTogether = daysWorked;
                                employeeOneId = employeeOne.Id;
                                employeeTwoId  = employeeTwo.Id;
                            }

                            employeePairs.Add(new EmployeesViewModel
                            {
                                EmployeeOneId = employeeOne.Id,
                                EmployeeTwoId = employeeTwo.Id,
                                ProjectId = projectGroup.Key,
                                DaysWorked = daysWorked
                            });
                        }
                    }
                }
            }

            return employeePairs.Where(e => e.EmployeeOneId == employeeOneId && e.EmployeeTwoId == employeeTwoId);
        }

        private bool HasOverlap(DateOnly startDateOne, DateOnly endDateOne, DateOnly startDateTwo, DateOnly endDateTwo) => 
            startDateOne < endDateTwo && startDateTwo < endDateOne;

        private DateOnly MaxDate(DateOnly dateOne, DateOnly dateTwo) => 
            dateOne > dateTwo ? dateOne : dateTwo;

        private DateOnly MinDate(DateOnly dateOne, DateOnly dateTwo) => 
            dateOne < dateTwo ? dateOne : dateTwo;
    }
}
