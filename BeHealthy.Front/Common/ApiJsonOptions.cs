using System.Text.Json;
using System.Text.Json.Serialization;

namespace BeHealthy.Front.Common;

/// <summary>
/// JSON options for calls to BeHealthy.API. The API serializes enums as strings
/// (JsonStringEnumConverter), so responses containing enum properties (UserRole,
/// AppointmentStatus, AllergySeverity, ...) fail to deserialize with the client default options
/// unless this converter is applied here too.
/// </summary>
public static class ApiJsonOptions
{
    public static readonly JsonSerializerOptions Default = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };
}
