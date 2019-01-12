namespace TEAM.Common
{
    using System.Configuration;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Cannot have instance and cannot be inherited.
    /// This class provides the feature to read information's from web/app configuration files based on key name.
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// Validate whether the input string is empty or not.
        /// </summary>
        /// <param name="value">Input value</param>
        /// <returns>Is empty or not</returns>
        #region
        public static bool IsEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            Regex nonBlank = new Regex("\\S");

            return nonBlank.IsMatch(value) == false;
        }
        #endregion

        /// <summary>
        /// Get App Settings value by Key name.
        /// </summary>
        /// <param name="key">Key Name</param>
        /// <returns>Value as string</returns>
        #region
        public static string GetAppSettingValueByKey(string key)
        {
            string keyValue = ConfigurationManager.AppSettings[key];

            if (IsEmpty(keyValue))
            {
                keyValue = string.Empty;
            }

            return keyValue;
        }
        #endregion

        /// <summary>
        /// Get Database Connection Settings value by Key name.
        /// </summary>
        /// <param name="key">Key Name</param>
        /// <returns>Database connection string as string</returns>
        #region
        public static string GetDatabaseConnectionStringByKey(string key)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[key] == null ? string.Empty : ConfigurationManager.ConnectionStrings[key].ConnectionString;

            if (IsEmpty(connectionString))
            {
                connectionString = string.Empty;
            }

            return connectionString;
        }
        #endregion
    }
}
