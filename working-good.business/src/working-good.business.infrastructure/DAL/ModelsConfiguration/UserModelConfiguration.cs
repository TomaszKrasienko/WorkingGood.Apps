using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using working_good.business.core.Models.Company;
using working_good.business.core.ValueObjects;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.infrastructure.DAL.ModelsConfiguration;

internal sealed class UserModelConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();
        builder
            .Property(x => x.Password)
            .HasConversion(x => x.Value, y => new Password(y))
            .IsRequired();
        builder.OwnsOne(x => x.FullName, options =>
        {
            options
                .Property(x => x.FirstName)
                .IsRequired()
                .HasColumnName(nameof(FullName.FirstName));
            options
                .Property(x => x.LastName)
                .IsRequired()
                .HasColumnName(nameof(FullName.LastName));
        });
        builder.OwnsOne(x => x.VerificationToken, options =>
        {
            options
                .Property(x => x.Token)
                .IsRequired()
                .HasColumnName("VerificationToken");
            options
                .Property(x => x.VerificationDate)
                .HasColumnName("VerificationTokenDate");
        });
        builder.OwnsOne(x => x.ResetPasswordToken, options =>
        {
            options
                .Property(x => x.Token)
                .HasColumnName("ResetToken");
            options
                .Property(x => x.Expiry)
                .HasColumnName("ResetTokenExpiry");
        });
        builder
            .Property(x => x.Role)
            .HasConversion(x => x.Value, y => new Role(y))
            .IsRequired();
    }
}