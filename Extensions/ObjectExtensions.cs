using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace MetricDashboard.Extensions
{
    public static class ObjectExtensions
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            IncludeFields = true
        };
        public static string Serialize<T>(this T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            
            return JsonSerializer.Serialize(obj, options);
        }
        public static T Deserialize<T>(this string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return default;

            return JsonSerializer.Deserialize<T>(jsonString, options)!;
        }
        public static T MaxByOrDefault<T,TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            if (!enumerable.Any())
                return default;
            return enumerable.MaxBy(keySelector);
        }
    }
}
