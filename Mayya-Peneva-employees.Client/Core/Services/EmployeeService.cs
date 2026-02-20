using Mayya_Peneva_employees.Client.Core.Entities;
using Mayya_Peneva_employees.Client.Core.Results;
using Mayya_Peneva_employees.Client.Models.ViewModels;

namespace Mayya_Peneva_employees.Client.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        public EmployeesResult GetPairEmployeesWorkedLongest(Dictionary<int, List<Employee>> employeesGroupsPerProject)
        {
            var result = new EmployeesResult();
            if (employeesGroupsPerProject.Count == 0)
            {
                result.Errors.Add("There are no employeesGroupsPerProject who worked together on the same project.");
                return result;
            }

            var employeesPairWorkDurations = new Dictionary<(int, int), int>();
            foreach (var project in employeesGroupsPerProject)
            {
                var projectEmployees = project.Value;
                for (int i = 0; i < projectEmployees.Count - 1; i++)
                {
                    for (int j = i + 1; j < projectEmployees.Count; j++)
                    {
                        var employeeOne = projectEmployees[i];
                        var employeeTwo = projectEmployees[j];

                        if (employeeOne.Id == employeeTwo.Id) continue;

                        var daysWorked = CalculateOverlapDays(employeeOne.DateFrom, employeeOne.DateTo, employeeTwo.DateFrom, employeeTwo.DateTo);
                        if (daysWorked > 0)
                        {
                            var pairKey = employeeOne.Id < employeeTwo.Id ? (employeeOne.Id, employeeTwo.Id) : (employeeTwo.Id, employeeOne.Id);
                            employeesPairWorkDurations[pairKey] = employeesPairWorkDurations.GetValueOrDefault(pairKey) + daysWorked;
                        }
                    }
                }
            }

            if (employeesPairWorkDurations.Count == 0)
            {
                result.Errors.Add("There are no employeesGroupsPerProject who worked together on any project.");
                return result;
            }

            var employeesPerProject = new List<EmployeesViewModel>();
            int maxDaysWorkedTogether = employeesPairWorkDurations.Values.Max();
            foreach (var pair in employeesPairWorkDurations.Where(p => p.Value == maxDaysWorkedTogether))
            {
                var (employeeOneId, employeeTwoId) = pair.Key;
                foreach (var projectGroup in employeesGroupsPerProject)
                {
                    if (projectGroup.Value.Any(e => e.Id == employeeOneId) && projectGroup.Value.Any(e => e.Id == employeeTwoId))
                    {
                        employeesPerProject.Add(new EmployeesViewModel(
                            employeeOneId,
                            employeeTwoId,
                            projectGroup.Key,
                            maxDaysWorkedTogether));
                    }
                }
            }

            result.EmployeesPerProject = employeesPerProject;
            return result;
        }

        private int CalculateOverlapDays(DateOnly startOne, DateOnly endOne, DateOnly startTwo, DateOnly endTwo)
        {
            var overlapStart = Math.Max(startOne.DayNumber, startTwo.DayNumber);
            int overlapEnd = Math.Min(endOne.DayNumber, endTwo.DayNumber);

            return overlapStart <= overlapEnd ? (overlapEnd - overlapStart + 1) : 0;
        }
    }
}
