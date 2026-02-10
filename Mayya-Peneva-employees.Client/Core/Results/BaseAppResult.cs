namespace Mayya_Peneva_employees.Client.Core.Results
{
    public class BaseAppResult
    {
        public ICollection<string> Errors { get; set; } = [];

        public bool IsSuccessful() => this.Errors.Count == 0;
    }
}
