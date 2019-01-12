using System;
using Newtonsoft.Json.Linq;
namespace TEAM.Common
{
    public static class JsonExtensions
    {
        public static bool TryParse(this object obj, out JObject jObject)
        {
            jObject = null;
            try
            {
                jObject = JObject.Parse(obj.ToString());
                if (jObject != null)
                {
                    return true;
                }
            }
            catch
            {
                // Absorb the exception.
            }
            return false;
        }

        /// <summary>
        /// Tries to parse a JObject to specified T type.
        /// </summary>
        /// <typeparam name="T">The T Type object</typeparam>
        /// <param name="jObject">The input job.</param>
        /// <param name="propertyName">The name of property.</param>
        /// <param name="value">The out value.</param>
        /// <returns>True if cast is successful.</returns>
        public static bool TryParse<T>(this JObject jObject, string propertyName, out T value)
        {
            if (jObject == null)
            {
                throw new ArgumentNullException(nameof(jObject), nameof(jObject) + " cannot be null.");
            }

            value = default(T);
            try
            {
                value = jObject.GetValue(propertyName, StringComparison.OrdinalIgnoreCase).ToObject<T>();
                if (value != null)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
