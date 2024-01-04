using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using working_good.business.core.Models.Company;
using working_good.business.core.ValueObjects;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.infrastructure.DAL.ModelsConfiguration;

internal sealed class EmployeeModelConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();
        builder
            .Property(x => x.Email)
            .HasConversion(x => x.Value, y => new Email(y))
            .IsRequired();
        builder
            .HasOne<User>(x => x.User)
            .WithOne()
            .HasForeignKey<User>(x => x.EmployeeId);

    }
}