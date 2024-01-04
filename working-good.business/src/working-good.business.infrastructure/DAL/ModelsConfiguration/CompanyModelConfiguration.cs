using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using working_good.business.core.Models.Company;
using working_good.business.core.ValueObjects;
using working_good.business.core.ValueObjects.Company;

namespace working_good.business.infrastructure.DAL.ModelsConfiguration;

internal sealed class CompanyModelConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();
        builder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, y => new Name(y))
            .IsRequired();
        builder
            .Property(x => x.IsOwner)
            .HasConversion(x => x.Value, y => new IsOwner(y))
            .IsRequired();
        builder
            .Property(x => x.SlaTimeSpan)
            .HasConversion(x => x.Value, y => new SlaTimeSpan(y));
        builder
            .Property(x => x.EmailDomain)
            .HasConversion(x => x.Value, y => new EmailDomain(y))
            .IsRequired();
        builder
            .HasMany<Employee>(x => x.Employees)
            .WithOne()
            .IsRequired();
    }
}