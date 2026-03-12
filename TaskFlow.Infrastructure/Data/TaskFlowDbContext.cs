using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Data
{
    public class TaskFlowDbContext: DbContext
    {
        public DbSet<ChangeEntity> Changes {  get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<CompanyUserEntity> CompanyUsers { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<ProjectUserEntity> ProjectUsers { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) : base(options)
        {

        }

    }
}
