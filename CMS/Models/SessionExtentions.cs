using System.Text.Json;

namespace CMS.Models
{
    /// <summary>
    /// Extension methods to store and retrieve session tokens as JSON.
    /// </summary>
    public static class SessionExtentions
    {
        /// <summary>
        /// Stores a session token as a JSON string.
        /// </summary>
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Retrieves a session token and deserializes it from JSON.
        /// </summary>
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }
}
