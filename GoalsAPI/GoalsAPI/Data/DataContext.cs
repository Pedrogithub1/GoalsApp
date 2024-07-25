using GoalsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GoalsAPI.Data
{
    public class DataContext : DbContext
    {
        #region Entities
            public DbSet<Goal> Goals { get; set; }
            public DbSet<TaskItem> TaskItems { get; set; }
        #endregion


        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Goal>()
               .HasMany(g => g.TaskItems)
               .WithOne()
               .HasForeignKey(t => t.GoalId)
               .OnDelete(DeleteBehavior.Cascade);

            DateTime startDate = DateTime.Now;

            modelBuilder.Entity<Goal>()
                .HasData(
                new Goal("Configurar plan de compensación")
                {
                    GoalId = 1,
                    Date = startDate.Date,
                    TotalTasks = 2,
                },
                new Goal("Meta 2")
                {
                    GoalId = 2,
                    Date = startDate.Date,
                    TotalTasks = 0,
                });
            modelBuilder.Entity<TaskItem>()
                .HasData(
                new TaskItem("Tarea 1")
                {
                    TaskItemId = 1,
                    Status = "Completada",
                    Date = startDate.Date,
                    GoalId = 1
                },
                new TaskItem("Tarea 2")
                {
                    TaskItemId = 2,
                    Status = "Abierta",
                    Date= startDate.Date,
                    GoalId = 1
                });


            base.OnModelCreating(modelBuilder);
        }

    }
}
