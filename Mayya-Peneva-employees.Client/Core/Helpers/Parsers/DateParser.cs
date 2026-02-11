using Mayya_Peneva_employees.Client.Core.Results;
using System.Globalization;

namespace Mayya_Peneva_employees.Client.Core.Helpers.Parsers
{
    public class DateParser : IDateParser
    {
        private static readonly string[] SupportedDateFormats = new[]
        {
            "M/d/yyyy", "MM/dd/yyyy", "d/M/yyyy", "dd/MM/yyyy", "yyyy/M/d", "yyyy/MM/dd",
            "M/d/yy", "MM/dd/yy", "d/M/yy", "dd/MM/yy", "yy/M/d", "yy/MM/dd",
            "M-d-yyyy", "MM-dd-yyyy", "d-M-yyyy", "dd-MM-yyyy", "yyyy-M-d", "yyyy-MM-dd",
            "M-d-yy", "MM-dd-yy", "d-M-yy", "dd-MM-yy", "yy-M-d", "yy-MM-dd",
            "M.d.yyyy", "MM.dd.yyyy", "d.M.yyyy", "dd.MM.yyyy", "yyyy.M.d", "yyyy.MM.dd",
            "M.d.yy", "MM.dd.yy", "d.M.yy", "dd.MM.yy", "yy.M.d", "yy.MM.dd",
            "M,d,yyyy", "MM,dd,yyyy", "d,M,yyyy", "dd,MM,yyyy", "yyyy,M,d", "yyyy,MM,dd",
            "M,d,yy", "MM,dd,yy", "d,M,yy", "dd,MM,yy", "yy,M,d", "yy,MM,dd",
            "M d yyyy", "MM dd yyyy", "d M yyyy", "dd MM yyyy", "yyyy M d", "yyyy MM dd",
            "M d yy", "MM dd yy", "d M yy", "dd MM yy", "yy M d", "yy MM dd",
            "d-MMM-yyyy", "d/MMM/yyyy", "d MMM yyyy", "d.MMM.yyyy",
            "d-MMM-yy", "d/MMM/yy", "d MMM yy", "d.MMM.yy",
            "d-MMM-y", "d/MMM/y", "d MMM y", "d.MMM.y",
            "dd-MMM-yyyy", "dd/MMM/yyyy", "dd MMM yyyy", "dd.MMM.yyyy",
            "dd-MMM-yy", "dd/MMM/yy", "dd MMM yy", "dd.MMM.yy",
            "MMM/dd/yyyy", "MMM-dd-yyyy", "MMM dd yyyy", "MMM.dd.yyyy", "MMM.dd.yyyy",
            "MMM/dd/yy", "MMM-dd-yy", "MMM dd yy", "MMM.dd.yy", "MMM.dd.yy"
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
