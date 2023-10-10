using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestWorkAPI.DB.Constants;
using TestWorkAPI.DB.Models;

namespace TestWorkAPI.DB.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.Users)
                .HasKey(user => user.Id);

            builder.Property(user => user.Name)
                .IsRequired();

            builder.Property(user => user.Age)
                .IsRequired();

            builder.Property(user => user.Email)
                .IsRequired();

        }
    }
}
