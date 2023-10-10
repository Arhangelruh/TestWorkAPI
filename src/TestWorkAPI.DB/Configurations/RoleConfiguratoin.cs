using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestWorkAPI.DB.Constants;
using TestWorkAPI.DB.Models;

namespace TestWorkAPI.DB.Configurations
{
    public class RoleConfiguratoin : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.Roles)
                .HasKey(role => role.Id);

            builder.Property(role => role.RoleName)
                .IsRequired();
        }
    }
}
