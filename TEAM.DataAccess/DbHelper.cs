using TEAM.Common;
using System.Data.Entity;

namespace TEAM.DAL
{
    /// <summary>
    /// Static class. Database helper class
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// Gets new database instance
        /// </summary>
        public static DbContext GetDatabase
        {
            get
            {
                string dbType = ConfigManager.GetAppSettingValueByKey("DefaultDatabase");
                if (string.Compare(dbType, "mysql", true) == 0)
                {
                    return new Contexts.MySqlDbContext(GetMySqlDbKeyName);
                }
                //else if (string.Compare(dbType, "mssql", true) == 0)
                //{
                //    return new MsSqlDbContext(GetMsSqlDbKeyName);
                //}
                //else if (string.Compare(dbType, "oracle", true) == 0)
                //{
                //    return new OracleDbContext(GetOracleDbKeyName);
                //}

                return null;
            }
        }

        /// <summary>
        /// Gets Microsoft SQL Server database key name from config file appSettings section
        /// </summary>
        public static string GetMsSqlDbKeyName => ConfigManager.GetAppSettingValueByKey("MsSqlDbConKeyName");

        /// <summary>
        /// Gets oracle database key name from config file appSettings section
        /// </summary>
        public static string GetOracleDbKeyName => ConfigManager.GetAppSettingValueByKey("OracleDbConKeyName");

        /// <summary>
        /// Gets MySql database key name from config file appSettings section
        /// </summary>
        public static string GetMySqlDbKeyName => ConfigManager.GetAppSettingValueByKey("MySqlDbConKeyName");
    }
}
