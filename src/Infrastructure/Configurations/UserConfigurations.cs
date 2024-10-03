using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(x => x.TelegramId)
                .IsRequired(false);

            builder.Property(x => x.LanguageCode)
                .HasMaxLength(2);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
