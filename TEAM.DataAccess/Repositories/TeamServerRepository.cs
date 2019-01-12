using TEAM.DAL.Repositories.Base;
using TEAM.Entity;

namespace TEAM.DAL.Repositories
{
    public class TeamServerRepository : Repository<TeamServer>, IRepository<TeamServer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamServerRepository" /> class.
        /// </summary>
        public TeamServerRepository() : base(DbHelper.GetDatabase)
        {
            // No implementation required
        }
    }
}
