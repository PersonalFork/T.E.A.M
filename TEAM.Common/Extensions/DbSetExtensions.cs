using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace TEAM.Common.Extensions
{
    public static class DbContextExtensions
    {
        public static IQueryable<T> LocalOrDatabase<T>(this DbContext context, Expression<Func<T, bool>> expression) where T : class
        {
            IEnumerable<T> localResults = context.Set<T>().Local.Where(expression.Compile());
            if (localResults.Any() == true)
            {
                return (localResults.AsQueryable());
            }

            IQueryable<T> databaseResults = context.Set<T>().Where(expression);

            return (databaseResults);
        }
    }
}
