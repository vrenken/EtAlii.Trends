namespace EtAlii.Trends;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(entity => entity.Name).IsRequired();
        builder.Property(entity => entity.Password).IsRequired();

        builder
            .HasMany(entity => entity.Diagrams)
            .WithOne(entity => entity.User)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
