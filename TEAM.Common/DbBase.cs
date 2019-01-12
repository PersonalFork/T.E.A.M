using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace TEAM.Common
{
    public abstract class DbBase : IDisposable
    {
        /// <summary>
        /// Flag which will carry whether this class can dispose or not.
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Pointer to an external unmanaged resource.
        /// </summary>
        private IntPtr handle = System.Runtime.InteropServices.Marshal.AllocHGlobal(100);

        /// <summary>
        /// Database object where client or sub applications has to use this to work with any database related operations.
        /// </summary>
        private Microsoft.Practices.EnterpriseLibrary.Data.Database db = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbBase" /> class.
        /// This will create the database connection based on web.config file settings by default.
        /// </summary>
        #region
        protected DbBase()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            this.db = factory.CreateDefault();
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DbBase" /> class with parameterized object.
        /// This will create the database connection based on DatabaseKeyName in web.config file.
        /// </summary>
        /// <param name="databaseKeyName">Database Key Name</param>
        #region
        protected DbBase(string databaseKeyName)
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            this.db = factory.Create(databaseKeyName);
        }
        #endregion

        /// <summary>
        /// Finalizes an instance of the <see cref="DbBase" /> class.
        /// </summary>
        #region
        ~DbBase()
        {
            this.Dispose(false);
        }
        #endregion

        /// <summary>
        /// Gets Database instance to manipulate database related operations.
        /// </summary>
        #region
        protected Microsoft.Practices.EnterpriseLibrary.Data.Database Db
        {
            get
            {
                return this.db;
            }
        }
        #endregion

        /// <summary>
        /// Dispose class
        /// </summary>
        #region
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Convert data table to specific entity model.
        /// </summary>
        /// <param name="data">Result set</param>
        /// <typeparam name="T">Entity model</typeparam>
        /// <returns>Collection of model</returns>
        #region
        protected static ICollection<T> ConvertDataTableToModels<T>(DataTable data) where T : class, new()
        {
            List<T> modelCollection = new List<T>();
            if (null != data && data.Rows.Count > 0)
            {
                T model;
                foreach (DataRow row in data.Rows)
                {
                    model = new T();

                    foreach (DataColumn column in data.Columns)
                    {
                        foreach (PropertyInfo property in model.GetType().GetProperties().ToList())
                        {
                            if (string.Compare(column.ColumnName, property.Name, true, CultureInfo.CurrentCulture) == 0)
                            {
                                switch (property.PropertyType.Name.ToLower(CultureInfo.CurrentCulture))
                                {
                                    case "int32":
                                        long val = Convert.ToInt32(row[column.ColumnName], CultureInfo.CurrentCulture);
                                        if (val < int.MinValue || val > int.MaxValue)
                                        {
                                            property.SetValue(model, Convert.ToInt32(row[column.ColumnName], CultureInfo.CurrentCulture), null);
                                        }
                                        else
                                        {
                                            property.SetValue(model, Convert.ToInt32(row[column.ColumnName], CultureInfo.CurrentCulture), null);
                                        }

                                        break;
                                    case "smallint":
                                        long value = Convert.ToInt32(row[column.ColumnName], CultureInfo.CurrentCulture);
                                        if (value < int.MinValue || value > int.MaxValue)
                                        {
                                            property.SetValue(model, Convert.ToInt64(row[column.ColumnName], CultureInfo.CurrentCulture), null);
                                        }
                                        else
                                        {
                                            property.SetValue(model, Convert.ToInt32(row[column.ColumnName], CultureInfo.CurrentCulture), null);
                                        }

                                        break;
                                    case "int64":
                                        property.SetValue(model, Convert.ToInt64(row[column.ColumnName], CultureInfo.CurrentCulture), null);
                                        break;
                                    case "string":
                                        property.SetValue(model, Convert.ToString(row[column.ColumnName], CultureInfo.CurrentCulture), null);
                                        break;
                                    case "datetime":
                                        property.SetValue(model, Convert.ToDateTime(row[column.ColumnName], CultureInfo.CurrentCulture), null);
                                        break;
                                    case "decimal":
                                        property.SetValue(model, Convert.ToDecimal(row[column.ColumnName], CultureInfo.CurrentCulture), null);
                                        break;
                                    default:
                                        break;
                                }

                                break;
                            }
                        }
                    }

                    // Add model to list
                    modelCollection.Add(model);
                }
            }

            return modelCollection;
        }
        #endregion

        /// <summary>
        /// Override method to dispose this class.
        /// </summary>
        /// <param name="isDisposing">Can Dispose this class</param>
        #region
        protected virtual void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                {
                    //// Disponse managed resources if any
                    this.db = null;
                }

                if (this.handle != IntPtr.Zero)
                {
                    this.handle = IntPtr.Zero;
                }

                this.isDisposed = true;
            }
        }
        #endregion
    }
}
