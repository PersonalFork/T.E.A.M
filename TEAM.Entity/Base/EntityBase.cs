using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TEAM.Entity.Base
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Interface: it defines the method for all entity models that will implement this interface
    /// </summary>
    public interface IEntityModel : System.IDisposable
    {
        /// <summary>
        /// Gets or sets Id field for a table
        /// </summary>
        [Key]
        int Id { get; set; }

        /// <summary>
        /// Gets or sets record created date and time for a record
        /// </summary>
        System.DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets record created user name or id
        /// </summary>
        [Required]
        [MaxLength(100)]
        string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets last modified date and time for a record
        /// </summary>
        System.DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets last modified user name or id
        /// </summary>
        [Required]
        [MaxLength(100)]
        string UpdatedBy { get; set; }
    }

    /// <summary>
    /// Abstract class: It implements IModel interface and will act as a base class for all entity models.
    /// </summary>
    public abstract class EntityBase : IEntityModel
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
        /// Initializes a new instance of the <see cref="EntityBase" /> class.
        /// </summary>
        #region
        protected EntityBase()
        {
            CreatedOn = System.DateTime.Now;
            UpdatedOn = System.DateTime.Now;
        }
        #endregion

        /// <summary>
        /// Finalizes an instance of the <see cref="EntityBase" /> class
        /// </summary>
        #region
        ~EntityBase()
        {
            Dispose(false);
        }
        #endregion

        /// <summary>
        /// Get output in XML format for a object
        /// </summary>
        /// <param name="value">List of objects</param>
        /// <returns>XML as string</returns>
        /// <typeparam name="T">Object type</typeparam>
        public static string GetXmlString<T>(IList<T> value)
        {
            XmlSerializer xmlSerializer = null;
            StringBuilder sb = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                if (null != value)
                {
                    xmlSerializer = new XmlSerializer(value.GetType());
                    xmlSerializer.Serialize(writer, value);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets or sets Id field for a table
        /// </summary>  
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the record is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets record created date and time for a record
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets record created user name or id
        /// </summary>

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets last modified date and time for a record
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets last modified user name or id
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Dispose class
        /// </summary>
        #region
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Override method to dispose this class.
        /// </summary>
        /// <param name="isDisposing">Can Dispose this class</param>
        #region
        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                if (handle != IntPtr.Zero)
                {
                    handle = IntPtr.Zero;
                }

                isDisposed = true;
            }
        }
        #endregion
    }
}