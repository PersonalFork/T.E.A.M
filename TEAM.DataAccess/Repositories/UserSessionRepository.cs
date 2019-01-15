using TEAM.DAL.Repositories.Base;
using TEAM.Entity;

namespace TEAM.DAL.Repositories
{
    public class UserSessionRepository : Repository<UserSession>, IRepository<UserSession>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionRepository" /> class.
        /// </summary>
        public UserSessionRepository() : base(DbHelper.GetDatabase)
        {

        }
    }
}
