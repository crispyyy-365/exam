using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(c => c.Name).IsRequired().HasColumnType("varchar(100)");
            builder.Property(c => c.Surname).IsRequired().HasColumnType("varchar(100)");
        }
    }
}
