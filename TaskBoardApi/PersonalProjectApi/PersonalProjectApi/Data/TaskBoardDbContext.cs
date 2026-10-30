using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PersonalProjectApi.Models;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.Enums;

namespace PersonalProjectApi.Data
{
    public class TaskBoardDbContext : IdentityDbContext
    {
        public TaskBoardDbContext(DbContextOptions<TaskBoardDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<BoardList> BoardLists { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Board> Boards { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditBuilder = new AuditLogBuilder();

            var modifiedEntries = ChangeTracker.Entries()
                .Where(log => log.Entity is not ActivityLog && 
                (log.State == EntityState.Added || log.State == EntityState.Modified || log.State == EntityState.Deleted))
                .ToList();

            foreach (var entry in modifiedEntries)
            {
                var logs = auditBuilder.BuildLogs(entry);
                if (logs.Any())
                {
                    ActivityLogs.AddRange(logs);
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("Id")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("CreatedAt")
                        .HasDefaultValueSql("GetDate()");
                }
            }

            modelBuilder.Entity<ActivityLog>()
                .Property(x => x.ActionType)
                .HasConversion<string>();

            modelBuilder.Entity<ActivityLog>().Ignore(a => a.Card);
            modelBuilder.Entity<ActivityLog>().Ignore(a => a.BoardList);
            modelBuilder.Entity<ActivityLog>().Ignore(a => a.Board);

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
