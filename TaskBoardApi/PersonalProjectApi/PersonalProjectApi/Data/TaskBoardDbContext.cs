using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Data
{
    public class TaskBoardDbContext : DbContext
    {
        public TaskBoardDbContext(DbContextOptions<TaskBoardDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<BoardList> BoardLists { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Board> Boards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Board>().HasData(
                new Board()
                {
                    Id = Guid.Parse("4637be30-a4e0-4ed0-99a6-83988fd44e00"),
                    Title = "My Tasks",
                    CreatedAt = DateTime.Now,
                });

            modelBuilder.Entity<BoardList>().HasData(
                new BoardList()
                {
                    Id = Guid.Parse("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"),
                    BoardId = Guid.Parse("4637be30-a4e0-4ed0-99a6-83988fd44e00"),
                    Title = "To Do",
                    Position = 0,
                },
                new BoardList()
                {
                    Id = Guid.Parse("faa6be83-b7c5-4130-98a3-c6fa0982c67f"),
                    BoardId = Guid.Parse("4637be30-a4e0-4ed0-99a6-83988fd44e00"),
                    Title = "In Progress",
                    Position = 1,
                },
                new BoardList()
                {
                    Id = Guid.Parse("d009636a-b61a-444d-a186-40aae85263c8"),
                    BoardId = Guid.Parse("4637be30-a4e0-4ed0-99a6-83988fd44e00"),
                    Title = "Finished",
                    Position = 2,
                });

            modelBuilder.Entity<Card>().HasData(
                new Card()
                {
                    Id = Guid.Parse("4cf6d1e5-bd29-4978-9b38-e4459c0bfa71"),
                    Title = "Make back for personal project",
                    Description = "Make an DB with all necesary data, connect it with API and front. Test it.",
                    BoardListId = Guid.Parse("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"),
                    DueDate = new DateTime(2026, 8, 30, 18, 0, 30),
                    Priority = TaskPriority.High,
                },
                new Card()
                {
                    Id = Guid.Parse("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                    Title = "Workout",
                    Description = "Go to the gym and eat well after that.",
                    BoardListId = Guid.Parse("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"),
                    DueDate = DateTime.Now,
                    Priority = TaskPriority.Medium,
                },
                new Card()
                {
                    Id = Guid.Parse("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                    Title = "Do household chores",
                    Description = "Clean my wardrobe...",
                    BoardListId = Guid.Parse("d009636a-b61a-444d-a186-40aae85263c8"),
                    DueDate = DateTime.Now,
                    Priority = TaskPriority.Low,
                }
            );
            modelBuilder.Entity<ActivityLog>()
                .HasOne(a => a.BoardList)
                .WithMany()
                .HasForeignKey(a => a.BoardListId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ActivityLog>()
                .HasOne(a => a.Card)
                .WithMany()
                .HasForeignKey(a => a.CardId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BoardList>()
                .HasOne(bl => bl.Board)
                .WithMany(b => b.BoardLists)
                .HasForeignKey(bl => bl.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.BoardList)
                .WithMany(bl => bl.Cards)
                .HasForeignKey(c => c.BoardListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
