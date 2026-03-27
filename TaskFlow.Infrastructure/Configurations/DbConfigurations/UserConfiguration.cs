using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Infrastructure.Configurations.DbConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("SYSDATETIME()");

            builder.Property(x => x.GlobalRole)
                .HasDefaultValue(GlobalRole.User);


            builder.HasMany(u => u.Companies).WithOne(c => c.User).HasForeignKey(c => c.UserId);

            builder.HasMany(u => u.Tasks).WithOne(t => t.User).HasForeignKey(t => t.UserId);

            builder.HasMany(u => u.Projects).WithOne(p => p.User).HasForeignKey(p => p.UserId);

            builder.HasMany(u => u.RefreshTokens).WithOne(r => r.User).HasForeignKey(r => r.UserId);

            builder.HasMany(u => u.Changes).WithOne(c => c.User).HasForeignKey(c => c.UserId);
        }
    }
}
