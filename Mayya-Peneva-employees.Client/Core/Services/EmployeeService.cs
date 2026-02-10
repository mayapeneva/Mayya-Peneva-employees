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

            var projectGroups = employees.GroupBy(e => e.ProjectId).ToArray();
            if (projectGroups.Length == 0)
            {
                result.Errors.Add("There are no employees who worked together on the same project.");
                return result;
            }

            var maxDaysWorkedTogether = 0;
            var allEmployeePairs = new List<(int EmployeeOneId, int EmployeeTwoId, int ProjectId, int DaysWorked)>();
            foreach (var projectGroup in projectGroups)
            {
                var projectEmployees = projectGroup.ToArray();
                for (int i = 0; i < projectEmployees.Length - 1; i++)
                {
                    for (int j = i + 1; j < projectEmployees.Length; j++)
                    {
                        var daysWorked = CalculateOverlapDays(
                            projectEmployees[i].DateFrom,
                            projectEmployees[i].DateTo,
                            projectEmployees[j].DateFrom,
                            projectEmployees[j].DateTo);

                        if (daysWorked > 0)
                        {
                            allEmployeePairs.Add((projectEmployees[i].Id, projectEmployees[j].Id, projectGroup.Key, daysWorked));
                            maxDaysWorkedTogether = Math.Max(maxDaysWorkedTogether, daysWorked);
                        }
                    }
                }
            }

            if (allEmployeePairs.Count == 0)
            {
                result.Errors.Add("There are no employees who worked together on the same project.");
                return result;
            }

            var maxDaysWorkedEmployeePairs = allEmployeePairs
                .Where(p => p.DaysWorked == maxDaysWorkedTogether)
                .ToList();

            result.EmployeeIdsWorkedTogether = maxDaysWorkedEmployeePairs
                .Select(p => new KeyValuePair<int, int>(p.EmployeeOneId, p.EmployeeTwoId))
                .Distinct();

            result.EmployeesPerProject = maxDaysWorkedEmployeePairs
                .Select(p => new EmployeesViewModel
                {
                    EmployeeOneId = p.EmployeeOneId,
                    EmployeeTwoId = p.EmployeeTwoId,
                    ProjectId = p.ProjectId,
                    DaysWorked = p.DaysWorked
                });

            return result;
        }

        private int CalculateOverlapDays(DateOnly startOne, DateOnly endOne, DateOnly startTwo, DateOnly endTwo)
        {
            var overlapStart = MaxDate(startOne, startTwo);
            var overlapEnd = MinDate(endOne, endTwo);

            return overlapStart <= overlapEnd ? (overlapEnd.DayNumber - overlapStart.DayNumber) + 1 : 0;
        }

        private DateOnly MaxDate(DateOnly dateOne, DateOnly dateTwo) =>
            dateOne > dateTwo ? dateOne : dateTwo;

        private DateOnly MinDate(DateOnly dateOne, DateOnly dateTwo) =>
            dateOne < dateTwo ? dateOne : dateTwo;
    }
}
