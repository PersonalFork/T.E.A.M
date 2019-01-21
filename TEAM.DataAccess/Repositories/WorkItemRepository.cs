using TEAM.DAL.Repositories.Base;
using TEAM.Entity;

namespace TEAM.DAL.Repositories
{
    public class WorkItemRepository : Repository<UserWorkItem>, IRepository<UserWorkItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkItemRepository" /> class.
        /// </summary>
        public WorkItemRepository() : base(DbHelper.GetDatabase)
        {
            // No implementation required
        }
    }
}