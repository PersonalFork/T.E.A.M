using TEAM.DAL.Repositories.Base;
using TEAM.Entity;

namespace TEAM.DAL.Repositories
{
    public class UserLoginRepository : Repository<UserLogin>
    {
        public UserLoginRepository() : base(DbHelper.GetDatabase)
        {

        }
    }
}
