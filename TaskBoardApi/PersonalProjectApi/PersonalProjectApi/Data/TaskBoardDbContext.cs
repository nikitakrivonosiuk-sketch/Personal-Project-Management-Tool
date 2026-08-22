using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Data
{
    public class TaskBoardDbContext : DbContext
    {
        public TaskBoardDbContext(DbContextOptions<TaskBoardDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Priority> Priorities { get; set; }
        public DbSet<BoardList> BoardLists { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BoardList>().HasData(
                new BoardList()
                {
                    Id = Guid.Parse("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"),
                    Title = "To Do",
                },
                new BoardList()
                {
                    Id = Guid.Parse("faa6be83-b7c5-4130-98a3-c6fa0982c67f"),
                    Title = "In Progress",
                },
                new BoardList()
                {
                    Id = Guid.Parse("d009636a-b61a-444d-a186-40aae85263c8"),
                    Title = "Finished",
                });

            modelBuilder.Entity<Card>().HasData(
                new Card()
                {
                    Id = Guid.Parse("4cf6d1e5-bd29-4978-9b38-e4459c0bfa71"),
                    Title = "Make back for personal project",
                    Description = "Make an DB with all necesary data, connect it with API and front. Test it.",
                    BoardListId = Guid.Parse("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"),
                    DueDate = new DateTime(2026, 8, 30, 18, 0, 30),
                    PriorityId = Guid.Parse("c1f2fa72-41e5-4dbf-bc93-19311c40a43e")
                },
                new Card()
                {
                    Id = Guid.Parse("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                    Title = "Workout",
                    Description = "Go to the gym and eat well after that.",
                    BoardListId = Guid.Parse("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"),
                    DueDate = DateTime.Now,
                    PriorityId = Guid.Parse("f4000d47-117a-42cd-81de-44a64d5a4219")
                },
                new Card()
                {
                    Id = Guid.Parse("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                    Title = "Do household chores",
                    Description = "Clean my wardrobe...",
                    BoardListId = Guid.Parse("d009636a-b61a-444d-a186-40aae85263c8"),
                    DueDate = DateTime.Now,
                    PriorityId = Guid.Parse("9e1572d6-328d-431d-a5c8-46d2fa119a64")
                }
            );

            modelBuilder.Entity<Priority>().HasData(
                new Priority()
                {
                    Id = Guid.Parse("9e1572d6-328d-431d-a5c8-46d2fa119a64"),
                    CardPriority = "Low",
                },
                new Priority()
                {
                    Id = Guid.Parse("f4000d47-117a-42cd-81de-44a64d5a4219"),
                    CardPriority = "Medium",
                },
                new Priority()
                {
                    Id = Guid.Parse("c1f2fa72-41e5-4dbf-bc93-19311c40a43e"),
                    CardPriority = "High",
                }
            );
        }
    }
}
