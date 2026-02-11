using Mayya_Peneva_employees.Client.Core.Results;

namespace Mayya_Peneva_employees.Client.Core.Helpers.Parsers
{
    public interface IDateParser
    {
        DateParseResult TryParseDate(string dateString, string fieldName, int employeeId);
    }
}
