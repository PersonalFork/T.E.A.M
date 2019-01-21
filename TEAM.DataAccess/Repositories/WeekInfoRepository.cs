using TEAM.DAL.Repositories.Base;
using TEAM.Entity;

namespace TEAM.DAL.Repositories
{
    public class WeekInfoRepository : Repository<WeekInfo>
    {
        public WeekInfoRepository() : base(DbHelper.GetDatabase)
        {
        }
    }
}
