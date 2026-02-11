using Mayya_Peneva_employees.Client.Core.Results;
using Microsoft.AspNetCore.Components.Forms;

namespace Mayya_Peneva_employees.Client.Core.Helpers.Validators
{
    public class FileValidator : IFileValidator
    {
        private const long MaxFileSize = 5 * 1024 * 1024;
        private const long MinFileSize = 1 * 1024;
        private readonly string[] AllowedExtensions = { ".csv" };

        public FileValidationResult Validate(IBrowserFile file)
        {
            var result = new FileValidationResult();
            var extension = Path.GetExtension(file.Name).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                result.Errors.Add($"Invalid file extension. Only .csv files are allowed.");
                return result;
            }

            if (file.Size < MinFileSize)
            {
                result.Errors.Add("File is too small. Please provide a valid CSV file.");
                return result;
            }

            if (file.Size > MaxFileSize)
            {
                result.Errors.Add($"File size exceeds the maximum allowed size of {MaxFileSize / (1024 * 1024)} MB.");
                return result;
            }

            if (!file.ContentType.Equals("text/csv", StringComparison.OrdinalIgnoreCase) &&
                !file.ContentType.Equals("application/vnd.ms-excel", StringComparison.OrdinalIgnoreCase))
            {
                result.Errors.Add("Invalid file type. Please upload a valid CSV file.");
                return result;
            }

            result.IsValid = true;
            return result;
        }
    }
}
