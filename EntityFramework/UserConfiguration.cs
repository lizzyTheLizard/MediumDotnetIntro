using System.Text.Json.Serialization;

namespace EntityFramework;

public class UserConfiguration

{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required UserConfigurationName Name { get; set; }
    public required Example Example { get; set; }

}

public enum UserConfigurationName
{
    Favorite
}
