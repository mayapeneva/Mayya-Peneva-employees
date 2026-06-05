namespace Mayya_Peneva_employees.Client.Core.Services
{
    public class EmployeePairInfo
    {
        public int TotalDaysWorked { get; set; }
        public HashSet<int> SharedProjectIds { get; set; } = new();
    }
}
