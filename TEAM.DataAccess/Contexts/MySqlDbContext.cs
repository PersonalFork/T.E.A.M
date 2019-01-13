using MySql.Data.EntityFramework;

using System.Data.Entity;

using TEAM.DAL.Migrations;
using TEAM.Entity;

namespace TEAM.DAL.Contexts
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MySqlDbContext : DbContext, IDbContext
    {
        // Use EntityFramework\Update-Database -Verbose in Package Manager Console.

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbContext" /> class.
        /// </summary>
        public MySqlDbContext()
            : base("DefaultDb")
        {
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MySqlDbContext, Configuration>("DefaultDb"));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbContext" /> class.
        /// </summary>
        /// <param name="nameOrConnectionString">Database connection string or configuration file key name</param>
        public MySqlDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MySqlDbContext, Configuration>(nameOrConnectionString));
        }

        public IDbSet<WorkItem> Tasks { get; set; }
        public IDbSet<UserSession> UserSessionList { get; set; }
        public IDbSet<TeamServer> TeamServers { get; set; }
        public IDbSet<UserInfo> UserInfoList { get; set; }
        public IDbSet<UserLogin> UserLogin { get; set; }
        public IDbSet<UserServerInfo> UserServerInfo { get; set; }

        /// <summary>
        /// Model creation event to make the relationship between models
        /// </summary>
        /// <param name="modelBuilder">Model builder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (null != modelBuilder)
            {
                // Implement your mapping code here
            }
        }
    }
}
