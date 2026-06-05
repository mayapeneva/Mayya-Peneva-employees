using Mayya_Peneva_employees.Client.Core.Entities;
using Mayya_Peneva_employees.Client.Core.Results;
using Mayya_Peneva_employees.Client.Models.ViewModels;

namespace Mayya_Peneva_employees.Client.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private int maxDaysWorkedTogetherOverall;
        private Dictionary<(int, int), EmployeePairInfo> pairsWorkedTogetherLongest;

        public EmployeesResult GetPairEmployeesWorkedLongest(Dictionary<int, List<Employee>> employeesGroupsPerProject)
        {
            var result = new EmployeesResult();
            if (employeesGroupsPerProject.Count == 0)
            {
                result.Errors.Add("There are no employee groups who worked together on the same project.");
                return result;
            }

            maxDaysWorkedTogetherOverall = 0;
            pairsWorkedTogetherLongest = new Dictionary<(int, int), EmployeePairInfo>();

            foreach (var (projectId, projectEmployees) in employeesGroupsPerProject)
            {
                this.CalculatePairOverlapsForProject(projectId, projectEmployees);
            }

            if (pairsWorkedTogetherLongest.Count == 0)
            {
                result.Errors.Add("There are no employee pairs who worked together on any project.");
                return result;
            }

            var employeesPerProject = new List<EmployeesViewModel>(pairsWorkedTogetherLongest.Count);
            foreach (var (pairKey, pairInfo) in pairsWorkedTogetherLongest)
            {
                var (employeeOneId, employeeTwoId) = pairKey;
                foreach (var projectId in pairInfo.SharedProjectIds)
                {
                    employeesPerProject.Add(new EmployeesViewModel(
                        employeeOneId,
                        employeeTwoId,
                        projectId,
                        pairInfo.TotalDaysWorked));
                }
            }

            result.EmployeesPerProject = employeesPerProject;

            return result;
        }

        private void CalculatePairOverlapsForProject(int projectId, List<Employee> projectEmployees)
        {
            if (projectEmployees.Count < 2)
                return;

            for (int i = 0; i < projectEmployees.Count - 1; i++)
            {
                var employeeOne = projectEmployees[i];
                for (int j = i + 1; j < projectEmployees.Count; j++)
                {
                    var employeeTwo = projectEmployees[j];
                    if (employeeOne.Id == employeeTwo.Id)
                        continue;

                    var daysWorked = CalculateOverlapDays(
                        employeeOne.DateFrom,
                        employeeOne.DateTo,
                        employeeTwo.DateFrom,
                        employeeTwo.DateTo);

                    if (daysWorked > 0)
                    {
                        var pairKey = employeeOne.Id < employeeTwo.Id
                            ? (employeeOne.Id, employeeTwo.Id)
                            : (employeeTwo.Id, employeeOne.Id);

                        if (daysWorked < maxDaysWorkedTogetherOverall)
                            continue;

                        if (daysWorked > maxDaysWorkedTogetherOverall)
                        {
                            maxDaysWorkedTogetherOverall = daysWorked;
                            pairsWorkedTogetherLongest.Clear();
                        }

                        if (pairsWorkedTogetherLongest.TryGetValue(pairKey, out var existingPair))
                        {
                            existingPair.SharedProjectIds.Add(projectId);
                        }
                        else
                        {
                            pairsWorkedTogetherLongest[pairKey] = new EmployeePairInfo
                            {
                                TotalDaysWorked = daysWorked,
                                SharedProjectIds = [projectId]
                            };
                        }
                    }
                }
            }
        }

        private static int CalculateOverlapDays(DateOnly startOne, DateOnly endOne, DateOnly startTwo, DateOnly endTwo)
        {
            var overlapStart = Math.Max(startOne.DayNumber, startTwo.DayNumber);
            var overlapEnd = Math.Min(endOne.DayNumber, endTwo.DayNumber);

            return overlapStart <= overlapEnd ? (overlapEnd - overlapStart + 1) : 0;
        }
    }
}
