using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Models;
using System.IO;

namespace TaskManagementApp.Data
{
    public class TaskDbContext : DbContext
    {
        public DbSet<TaskModel> Tasks { get; set; }

        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "tasks.db");
                optionsBuilder.UseSqlite($"Filename={dbPath}");
            }
        }
    }
}
