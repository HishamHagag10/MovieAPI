
namespace MovieAPI.Validations
{
    public class MaxFileSizeAttribute: ValidationAttribute
    {
        public long MaxSizeInByte { get; set; }
        public MaxFileSizeAttribute(long maxSizeInByte) {
            MaxSizeInByte = maxSizeInByte;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;
            var file = (IFormFile) value;
            if (file == null) return null;
            if (file.Length > MaxSizeInByte)
            {
                return new ValidationResult(ErrorMessage ?? $"The File Size Must be less than or equal {MaxSizeInByte} Byte");
            }
            return ValidationResult.Success;
        }
    }
}
