using Newtonsoft.Json;

namespace VFi.NetDevPack.Serialize;

public class Serialize
{
    public static string JsonSerializeObject<T>(T? obj)
    {
        if (obj == null)
        {
            return string.Empty;
        }

        return JsonConvert.SerializeObject(obj, Formatting.None,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
    }

    public static T? JsonDeserializeObject<T>(string? json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(json);
    }

    public static object? JsonDeserializeObject(string? json, Type type)
    {
        return JsonConvert.DeserializeObject(json, type);
    }
}
