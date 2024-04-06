using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace MetricDashboard.Extensions
{
    public static class ObjectExtensions
    {
        public static string Serialize<T>(this T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return JsonSerializer.Serialize(obj);
        }
        public static T Deserialize<T>(this string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return default;

            return JsonSerializer.Deserialize<T>(jsonString)!;
        }
    }
}
