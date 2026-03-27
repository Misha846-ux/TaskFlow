using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums.TaskEnums;

namespace TaskFlow.Infrastructure.Configurations.DbConfigurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.TaskName)
                .IsUnique();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("SYSDATETIME()");

            builder.Property(x => x.Status)
                .HasDefaultValue(TaskFlow.Domain.Enums.TaskEnums.TaskStatus.Waiting);

            builder.HasOne(t => t.User).WithMany(u => u.Tasks).HasForeignKey(t => t.UserId);

            builder.HasOne(t => t.Project).WithMany(p => p.Tasks).HasForeignKey(t => t.ProjectId);
        }
    }
}
