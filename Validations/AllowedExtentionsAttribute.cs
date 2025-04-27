
namespace MovieAPI.Validations
{
    public class AllowedExtentionsAttribute : ValidationAttribute
    {
        public IEnumerable<string> AllowedExtentions { get; set; }
        public AllowedExtentionsAttribute(string allowedExtentions)
        {
            AllowedExtentions = allowedExtentions.ToLower().Split(',');
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;
            var file = (IFormFile)value;
            if (file == null) return null;
            if (!AllowedExtentions.Contains(Path.GetExtension(file.FileName.ToLower())))
            {
                return new ValidationResult(ErrorMessage??$"{string.Join('-', AllowedExtentions)} Are only Allowed Extentions.");
            }
            return ValidationResult.Success;
        }
    }
}
