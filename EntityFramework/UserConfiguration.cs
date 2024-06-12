using System.Text.Json.Serialization;

namespace EntityFramework;

//An exaple for a 1:1-Relation
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
