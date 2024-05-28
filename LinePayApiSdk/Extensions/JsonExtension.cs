using System.Text.Json;
using System.Text.Json.Serialization;

namespace LinePayApiSdk.Extensions
{
    public static class JsonExtension
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, SerializerOptions);
        }

        public static bool TryParseJson<T>(this string json, out T obj, out string errMsg) where T : class
        {
            errMsg = string.Empty;
            try
            {
                obj = JsonSerializer.Deserialize<T>(json);
                return true;
            }
            catch(Exception e)
            {
                obj = default;
                errMsg = e.Message;
                return false;
            }
        }
    }
}