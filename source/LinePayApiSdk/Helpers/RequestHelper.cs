using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;

namespace LinePayApiSdk.Helpers
{
    public static class RequestHelper
    {
        public static IEnumerable<KeyValuePair<string, string>> GenerateParameters<TRequestParameter>(
            TRequestParameter parameterObj)
        {
            if(parameterObj == null)
                yield break;
            var type = parameterObj.GetType();
            foreach (var property in type.GetProperties())
            {
                var propValue = property.GetValue(parameterObj);
                if (propValue == null)
                    continue;
                var propName = property.GetCustomAttributes<JsonPropertyNameAttribute>().FirstOrDefault()?.Name ??
                               throw new CustomAttributeFormatException("JsonPropertyNameAttribute not found.");
                if (propValue is IEnumerable items)
                {
                    foreach (var item in items)
                    {
                        yield return new KeyValuePair<string, string>(propName, item.ToString());
                    }
                }
                else
                {
                    yield return new KeyValuePair<string, string>(propName, propValue.ToString());
                }
            }
        }
    }
}