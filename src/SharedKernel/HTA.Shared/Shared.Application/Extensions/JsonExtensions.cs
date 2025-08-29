using Newtonsoft.Json;

namespace Shared.Application
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
        {
            string json = JsonConvert.SerializeObject(obj) ?? string.Empty;
            return json;
        }

        public static string ToJson(this object obj, JsonSerializerSettings settings)
        {
            string json = JsonConvert.SerializeObject(obj, settings) ?? string.Empty;
            return json;
        }

        public static string ToJson(this object obj, Formatting formatting)
        {
            string json = JsonConvert.SerializeObject(obj, formatting) ?? string.Empty;
            return json;
        }

        public static string ToJson(this object obj, Formatting formatting, params JsonConverter[] converters)
        {
            string json = JsonConvert.SerializeObject(obj, formatting, converters) ?? string.Empty;
            return json;
        }

        public static string ToJson(this object obj, params JsonConverter[] converters)
        {
            string json = JsonConvert.SerializeObject(obj, converters) ?? string.Empty;
            return json;
        }

        public static string ToJson(this object obj, Type type, JsonSerializerSettings settings)
        {
            string json = JsonConvert.SerializeObject(obj, type, settings) ?? string.Empty;
            return json;
        }

        public static string ToJson(this object obj, Type type, Formatting formatting, JsonSerializerSettings settings)
        {
            string json = JsonConvert.SerializeObject(obj, type, formatting, settings) ?? string.Empty;
            return json;
        }

        public static T? FromJson<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static T? FromJson<T>(this string jsonString, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, settings);
        }

        public static T? FromJson<T>(this string jsonString, params JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, converters);
        }

        public static object? FromJson(this string jsonString)
        {
            return JsonConvert.DeserializeObject(jsonString);
        }

        public static object? FromJson(this string jsonString, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(jsonString, settings);
        }

        public static object? FromJson(this string jsonString, Type type)
        {
            return JsonConvert.DeserializeObject(jsonString, type);
        }

        public static object? FromJson(this string jsonString, Type? type, JsonSerializerSettings? settings)
        {
            return JsonConvert.DeserializeObject(jsonString, type, settings);
        }

        public static object? FromJson(this string jsonString, Type type, params JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject(jsonString, type, converters);
        }
    }
}