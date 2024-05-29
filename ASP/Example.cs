using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ASP
{
    // A model or DTO can be modelled as record (Value-Equallity, Generated Properties incl. Destructor, ToString())
    // Model-Validation can be done with Annotations like Required, Range and custom attributes like DateInRange
    // Model-Serialization can be overwritten using JsonConverters
    public record class Example(
        [Required] Guid Id,
        [Required][DateRange(FromYear = 2000)] DateOnly Date,
        [Required][Range(-20, 55, ErrorMessage = "Value must be between -20 and 55")] int Value,
        [property: JsonConverter(typeof(SummaryConverter))] string? Note
        )
    { }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    file class DateRangeAttribute : ValidationAttribute
    {
        public int FromYear { get; set; } = DateOnly.MinValue.Year;
        public int ToYear { get; set; } = DateOnly.MaxValue.Year;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var year = ((DateOnly)value!).Year;
            if (year < FromYear)
            {
                var errorMessage = $"Date before be in or after year {FromYear}";
                return new ValidationResult(errorMessage);
            }
            if (year > ToYear)
            {
                var errorMessage = $"Date before be in or before year {ToYear}";
                return new ValidationResult(errorMessage);
            }
            return ValidationResult.Success;
        }

    }

    file class SummaryConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var input = reader.GetString();
            if (input == null) return "";
            if (input.StartsWith("In Short")) return input.Substring(10);
            else return input;
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"In Short: {value}");
        }
    }
}
