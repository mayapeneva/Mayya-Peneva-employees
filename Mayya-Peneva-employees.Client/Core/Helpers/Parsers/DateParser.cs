using Mayya_Peneva_employees.Client.Core.Results;
using System.Globalization;

namespace Mayya_Peneva_employees.Client.Core.Helpers.Parsers
{
    public class DateParser : IDateParser
    {
        public DateParseResult TryParseDate(string dateString, string fieldName, int employeeId)
        {
            var result = new DateParseResult();

            if (string.IsNullOrWhiteSpace(dateString))
            {
                result.Errors.Add($"Empty date value for employee with Id {employeeId}. Please provide a valid {fieldName}.");
                return result;
            }

            var trimmedDate = dateString.Trim();

            var globalDateFormats = GetGlobalDateFormats().ToArray();
            if (DateOnly.TryParseExact(trimmedDate, globalDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                result.ParsedDate = parsedDate;
                return result;
            }

            if (DateOnly.TryParse(trimmedDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var defaultDate))
            {
                result.ParsedDate = defaultDate;
                return result;
            }

            result.Errors.Add($"Invalid date format '{trimmedDate}' for employee with Id {employeeId}. Please provide a valid {fieldName}.");
            return result;
        }

        private IEnumerable<string> GetGlobalDateFormats()
        {
            var allCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            string[] priorityCultureNames = { "en-GB", "de-DE", "fr-FR", "en-BG" };

            var priorityCultures = allCultures.Where(c => priorityCultureNames.Contains(c.Name));
            var otherCultures = allCultures.Where(c => !priorityCultureNames.Contains(c.Name));

            return GetPatterns(priorityCultures)
                .Concat(GetPatterns(otherCultures))
                .Distinct();
        }

        private IEnumerable<string> GetPatterns(IEnumerable<CultureInfo> cultures) =>
                        cultures.SelectMany(c => c.DateTimeFormat.GetAllDateTimePatterns('d'))
                               .Concat(cultures.SelectMany(c => c.DateTimeFormat.GetAllDateTimePatterns('D')));
    }
}
