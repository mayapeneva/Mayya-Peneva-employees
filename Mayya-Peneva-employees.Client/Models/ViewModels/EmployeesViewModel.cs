namespace Mayya_Peneva_employees.Client.Models.ViewModels
{
    public class EmployeesViewModel
    {
        public EmployeesViewModel(int employeeOneId, int employeeTwoId, int projectId, int daysWorked)
        {
            this.EmployeeOneId = employeeOneId;
            this.EmployeeTwoId = employeeTwoId;
            this.ProjectId = projectId;
            this.DaysWorked = daysWorked;
        }

        public int EmployeeOneId { get; private set; }

        public int EmployeeTwoId { get; private set; }

        public int ProjectId { get; private set; }

        public int DaysWorked { get; set; }
    }
}
