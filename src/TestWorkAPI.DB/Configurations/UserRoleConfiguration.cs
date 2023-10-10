using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestWorkAPI.DB.Constants;
using TestWorkAPI.DB.Models;

namespace TestWorkAPI.DB.Configurations
{
    /// <summary>
    /// EF Configuration for UserRole entity.
    /// </summary>
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        /// <inheritdoc/>       
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.UserRole)
                .HasKey(UserRole => UserRole.Id);

            builder.HasOne(UserRole => UserRole.User)
                .WithMany(user => user.UsersRoles)
                .HasForeignKey(UserRole => UserRole.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(UserRole => UserRole.Role)
                .WithMany(role => role.UsersRoles)
                .HasForeignKey(UserRole => UserRole.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
