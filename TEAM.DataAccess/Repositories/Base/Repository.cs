using System.Data.Entity;
using System.Data.Entity.Migrations;
using TEAM.Entity.Base;

namespace TEAM.DAL.Repositories.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Interface class which will implements Insert, Delete, SearchFor, GetAll and GetById methods to access the entity.
    /// </summary>
    /// <typeparam name="T">Entity class object</typeparam>
    public interface IRepository<T> : IDisposable where T : EntityBase
    {
        /// <summary>
        /// Add entity to Database Context.
        /// </summary>
        /// <param name="entity">Entity class that needs to be added into Database Context</param>
        /// <returns>Integer value representing number of rows affected</returns>
        int Insert(T entity);

        /// <summary>
        /// Add entity list
        /// </summary>
        /// <param name="entityCollection">List of entity</param>
        /// <returns>Integer value representing number of rows affected</returns>
        int Insert(IList<T> entityCollection);

        /// <summary>
        /// Soft delete record by updating IsActive field to false
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Integer value returns whether soft delete is successfull or not</returns>
        int SoftDelete(int id);

        /// <summary>
        /// Delete entry of the entity from Database Context.
        /// </summary>
        /// <param name="entity">Entity class that needs to be deleted from Database Context</param>
        /// <returns>Integer value representing number of rows affected</returns>
        int Delete(T entity);

        /// <summary>
        /// Bulk delete entity items
        /// </summary>
        /// <param name="entityCollection">T entity collection</param>
        /// <returns>Integer value representing number of rows affected</returns>
        int Delete(IList<T> entityCollection);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        /// <returns>Entity object</returns>
        T Find(params object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Entity object</returns>
        T Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <returns>Queryable list</returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets objects from database with filting and paging.
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns>Queryable list</returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        /// <returns>Boolean value that indicates whether the object is available on the database or not</returns>
        bool Contains(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get all records for the entity.
        /// </summary>
        /// <returns>Returns all the details for the entity</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Get entity details by id.
        /// </summary>
        /// <param name="id">Unique id value of your table primary key</param>
        /// <returns>Entity :: returns entity details when matches. Otherwise return empty entity</returns>
        T GetById(int id);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <returns>Integer value representing number of records affected</returns>
        int Update(T entity);

        /// <summary>
        /// Bulk update entities
        /// </summary>
        /// <param name="entityCollection">List of entities to update</param>
        /// <returns>Boolean value indicating whether the update operation is successful or not</returns>
        int Update(IList<T> entityCollection);
    }

    /// <summary>
    /// This class cannot be inherited. Create the instance of this class with your custom Database Context class name which will provide you the option to perform all database related operations with LINQ enable.
    /// </summary>
    /// <typeparam name="T">Entity class object</typeparam>
    public class Repository<T> : IRepository<T> where T : EntityBase
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
        /// Object will get added into Database Context using this object.
        /// </summary>
        private DbSet<T> entitySet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}" /> class.
        /// </summary>
        /// <param name="dataContext">Database Context class</param>
        #region
        public Repository(DbContext dataContext)
        {
            entitySet = dataContext.Set<T>();
            DataContext = dataContext;
        }
        #endregion

        /// <summary>
        /// Finalizes an instance of the <see cref="Repository{T}" /> class
        /// </summary>
        #region
        ~Repository()
        {
            Dispose(false);
        }
        #endregion

        /// <summary>
        /// Gets or sets the underlying DBContext instance
        /// </summary>
        private DbContext DataContext { get; set; }

        /// <summary>
        /// Create or insert new record into a given table
        /// </summary>
        /// <param name="entity">Inpyt entity</param>
        /// <returns>Integer value that returns the saved record id</returns>
        #region
        public int Insert(T entity)
        {
            if (entity != null)
            {
                entitySet.AddOrUpdate(entity);
                return DataContext.SaveChanges();
            }

            return 0;
        }
        #endregion

        /// <summary>
        /// Bulk create or insert new records into a give table
        /// </summary>
        /// <param name="entityCollection">Input enitty list</param>
        /// <returns>Integer value that returns the no. of records that have got created or inserted</returns>
        #region
        public int Insert(IList<T> entityCollection)
        {
            if (entityCollection != null && entityCollection.Count > 0)
            {
                entitySet.AddOrUpdate(entityCollection.ToArray());
                return DataContext.SaveChanges();
            }

            return 0;
        }
        #endregion

        /// <summary>
        /// Soft delete record by updating IsActive field to false
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Integer value returns whether soft delete is successfull or not</returns>
        #region
        public int SoftDelete(int id)
        {
            T softDeleteEntity = GetById(id);
            if (softDeleteEntity != null && softDeleteEntity.Id > 0)
            {
                softDeleteEntity.IsActive = false;
                return Update(softDeleteEntity);
            }

            return 0;
        }
        #endregion

        /// <summary>
        /// Delete record from table by entity
        /// </summary>
        /// <param name="entity">Delete entity object</param>
        /// <returns>Integer value that returns whether the record gets deleted or not</returns>
        #region
        public int Delete(T entity)
        {
            if (entity != null)
            {
                T deleteEntity = GetById(entity.Id);
                if (deleteEntity != null && deleteEntity.Id > 0)
                {
                    entitySet.Remove(deleteEntity);
                    return DataContext.SaveChanges();
                }
            }

            return 0;
        }
        #endregion

        /// <summary>
        /// Bulk delete records from table by list of entities
        /// </summary>
        /// <param name="entityCollection">Delete entity object list</param>
        /// <returns>Integer value that returns whether the records gets deleted or not</returns>
        #region
        public int Delete(IList<T> entityCollection)
        {
            if (entityCollection != null && entityCollection.Count > 0)
            {
                foreach (T entity in entityCollection)
                {
                    Delete(entity);
                }

                return entityCollection.Count;
            }

            return 0;
        }
        #endregion

        /// <summary>
        /// Find object by key value pairs
        /// </summary>
        /// <param name="keys">Key value pair</param>
        /// <returns>Entity object</returns>
        #region
        public T Find(params object[] keys)
        {
            return entitySet.Find(keys);
        }
        #endregion

        /// <summary>
        /// Find object by expression
        /// </summary>
        /// <param name="predicate">Expression for search</param>
        /// <returns>Entity object</returns>
        #region
        public T Find(Expression<Func<T, bool>> predicate)
        {
            return entitySet.FirstOrDefault(predicate);
        }
        #endregion


        public T FindLocal(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> localResults = entitySet.Local.Where(predicate.Compile());
            if (localResults.Any() == true)
            {
                return (localResults.FirstOrDefault());
            }
            return entitySet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Filter records by criteria
        /// </summary>
        /// <param name="predicate">Filter criteria</param>
        /// <returns>Queryable list</returns>
        #region
        public IQueryable<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return entitySet.Where(predicate).AsQueryable<T>();
        }
        #endregion

        public IQueryable<T> FilterLocal(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> localResults = entitySet.Local.Where(predicate.Compile());
            if (localResults.Any() == true)
            {
                return (localResults.AsQueryable());
            }
            return entitySet.Where(predicate).AsQueryable<T>();
        }

        /// <summary>
        /// Filter records by criteria and page size
        /// </summary>
        /// <param name="filter">Filter criteria</param>
        /// <param name="total">Total records</param>
        /// <param name="index">Page index</param>
        /// <param name="size">Page size</param>
        /// <returns>Queryable list</returns>
        #region
        public IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            IQueryable<T> resetSet = filter != null ? entitySet.Where(filter).AsQueryable() : entitySet.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }
        #endregion

        /// <summary>
        /// Check whether the enity is exist with given criteria
        /// </summary>
        /// <param name="predicate">Search criteria</param>
        /// <returns>Boolean value indicates whether the object is found in database or not</returns>
        #region
        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            return entitySet.Count(predicate) > 0;
        }
        #endregion

        /// <summary>
        /// Get all objects from database
        /// </summary>
        /// <returns>All entity objects</returns>
        #region
        public IQueryable<T> GetAll()
        {
            return entitySet.AsQueryable();
        }
        #endregion

        /// <summary>
        /// Get object by id
        /// </summary>
        /// <param name="id">Object id</param>
        /// <returns>Entity object</returns>
        #region
        public T GetById(int id)
        {
            return entitySet.Where(f => f.Id == id).FirstOrDefault();
        }
        #endregion

        /// <summary>
        /// Update object
        /// </summary>
        /// <param name="entity">Update entity object</param>
        /// <returns>Integer value indicates whether the object is deleted from database or not</returns>
        #region
        public int Update(T entity)
        {
            entitySet.AddOrUpdate(entity);
            return DataContext.SaveChanges();
        }
        #endregion

        /// <summary>
        /// Bulk update entity obejcts
        /// </summary>
        /// <param name="entityCollection">Entity list</param>
        /// <returns>Integer value indicates whether the object is deleted from database or not</returns>
        public int Update(IList<T> entityCollection)
        {
            entitySet.AddOrUpdate(entityCollection.ToArray());
            return DataContext.SaveChanges();
        }

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
                if (isDisposing)
                {
                    entitySet = null;
                    if (DataContext != null)
                    {
                        DataContext.Dispose();
                    }
                }

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
