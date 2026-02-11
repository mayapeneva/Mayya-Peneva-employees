using Mayya_Peneva_employees.Client.Core.Results;
using System.Globalization;

namespace Mayya_Peneva_employees.Client.Core.Helpers.Parsers
{
    public class DateParser : IDateParser
    {
        private static readonly string[] SupportedDateFormats = new[]
        {
            "yyyy-MM-dd",           // 2020-01-15
            "dd/MM/yyyy",           // 15/01/2020
            "MM/dd/yyyy",           // 01/15/2020
            "dd.MM.yyyy",           // 15.01.2020
            "yyyy/MM/dd",           // 2020/01/15
            "dd-MM-yyyy",           // 15-01-2020
            "M/d/yyyy",             // 1/5/2020
            "d/M/yyyy",             // 5/1/2020
            "MMM dd, yyyy",         // Jan 15, 2020
            "dd MMM yyyy"           // 15 Jan 2020
        };

        public DateParseResult TryParseDate(string dateString, string fieldName, int employeeId)
        {
            var result = new DateParseResult();

            if (string.IsNullOrWhiteSpace(dateString))
            {
                result.Errors.Add($"Empty date value for employee with Id {employeeId}. Please provide a valid {fieldName}.");
                return result;
            }

            var trimmedDate = dateString.Trim();

            var dateTimeFormatInfo = new DateTimeFormatInfo();
            var allDateTimePatterns = dateTimeFormatInfo.GetAllDateTimePatterns();
            if (DateOnly.TryParseExact(trimmedDate, allDateTimePatterns, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                result.ParsedDate = parsedDate;
                return result;
            }

            foreach (var format in SupportedDateFormats)
            {
                if (DateOnly.TryParseExact(trimmedDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var supportedParsedDate))
                {
                    result.ParsedDate = supportedParsedDate;
                    return result;
                }
            }

            if (DateOnly.TryParse(trimmedDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var defaultDate))
            {
                result.ParsedDate = defaultDate;
                return result;
            }

            result.Errors.Add($"Invalid date format '{trimmedDate}' for employee with Id {employeeId}. Please provide a valid {fieldName}.");
            return result;
        }
    }
}
