namespace EtAlii.Trends;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Password).IsRequired();

        builder
            .HasMany(e => e.Diagrams)
            .WithOne(e => e.User)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
