using System.Text.Json.Serialization;

namespace PersonalProjectApi.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserLevelAccess 
    {
        Reader = 0,
        Editor = 1,
        Admin = 2,
    }
}
