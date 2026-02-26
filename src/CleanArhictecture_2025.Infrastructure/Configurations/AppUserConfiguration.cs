using CleanArhictecture_2025.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArhictecture_2025.Infrastructure.Configurations;
public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.Property(u => u.FirstName).HasMaxLength(50);
        builder.Property(u => u.LastName).HasMaxLength(50);
        builder.Property(u => u.UserName).HasMaxLength(15);
        builder.Property(u => u.Email).HasMaxLength(255);
    }
}

// configuration da veri tabanıma kaydederken kolonların nasıl duracağını veya girdilerdeki validasyonları belirtebiliyorum. 