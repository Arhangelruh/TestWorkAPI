using Microsoft.EntityFrameworkCore;
using TestWorkAPI.DB.Configurations;
using TestWorkAPI.DB.Models;

namespace TestWorkAPI.DB.Context
{
    /// <summary>
    /// Database context
    /// </summary>
    public class WorkAPIContext : DbContext
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="options"></param>
        public WorkAPIContext(DbContextOptions<WorkAPIContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Users
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Roles.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// UserRoles.
        /// </summary>
        public DbSet<UserRole> UserRole { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.ApplyConfiguration(new RoleConfiguratoin());

            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
