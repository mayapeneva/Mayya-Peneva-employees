namespace Mayya_Peneva_employees.Client.Core.Results
{
    public abstract class BaseAppResult
    {
        public ICollection<string> Errors { get; set; } = [];

        public bool IsSuccessful() => Errors.Count == 0;
    }
}
