using Mayya_Peneva_employees.Client.Core.Entities;
using Mayya_Peneva_employees.Client.Core.Results;
using Mayya_Peneva_employees.Client.Models.ViewModels;

namespace Mayya_Peneva_employees.Client.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        public EmploeeysResult GetPairEmployeesWorkedLongest(IEnumerable<Employee> employees)
        {
            var result = new EmploeeysResult();

            var employeesWorkedTogether = new List<KeyValuePair<int, int>>();
            var highestDaysWorkedTogether = 0;

            var employeesPerProject = new List<EmployeesViewModel>();

            var projectGroups = employees.GroupBy(e => e.ProjectId);
            foreach (var projectGroup in projectGroups)
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
                                employeesWorkedTogether.Clear();
                                employeesWorkedTogether.Add(new KeyValuePair<int, int>(employeeOne.Id, employeeTwo.Id));
                            }
                            else if (daysWorked == highestDaysWorkedTogether)
                            {
                                if (!employeesWorkedTogether.Any(pair =>
                                    (pair.Key == employeeOne.Id && pair.Value == employeeTwo.Id) ||
                                    (pair.Key == employeeTwo.Id && pair.Value == employeeOne.Id)))
                                        employeesWorkedTogether.Add(new KeyValuePair<int, int>(employeeOne.Id, employeeTwo.Id));
                            }

                            employeesPerProject.Add(new EmployeesViewModel
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

            if (employeesWorkedTogether.Count == 0)
            {
                result.Errors.Add("There are no employees who worked together on the same project.");
                return result;
            }

            result.EmployeeIdsWorkedTogether = employeesWorkedTogether;
            result.EmployeesPerProject = employeesPerProject
                .Where(e => employeesWorkedTogether
                    .Any(pair => pair.Key == e.EmployeeOneId && pair.Value == e.EmployeeTwoId 
                            || pair.Key == e.EmployeeTwoId && pair.Value == e.EmployeeOneId));

            return result;
        }

        private bool HasOverlap(DateOnly startDateOne, DateOnly endDateOne, DateOnly startDateTwo, DateOnly endDateTwo) =>
            startDateOne <= endDateTwo && startDateTwo <= endDateOne;

        private DateOnly MaxDate(DateOnly dateOne, DateOnly dateTwo) =>
            dateOne > dateTwo ? dateOne : dateTwo;

        private DateOnly MinDate(DateOnly dateOne, DateOnly dateTwo) =>
            dateOne < dateTwo ? dateOne : dateTwo;
    }
}
