using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PersonalProjectApi.Models.Domain;

namespace PersonalProjectApi.Data
{
    public class AuditLogBuilder
    {
        Guid? GetBoardIdForCard(EntityEntry entry, Guid boardListId)
        {
            var context = entry.Context;
            var boardList = context.Set<BoardList>().Local.FirstOrDefault(bl => bl.Id == boardListId)
                            ?? context.Set<BoardList>().Find(boardListId);
            return boardList?.BoardId;
        }
        public List<ActivityLog> BuildLogs(EntityEntry entry)
        {
            var logs = new List<ActivityLog>();

            if (entry.State == EntityState.Modified)
            {
                // Card logs
                if (entry.Entity is Card card)
                {
                    if (entry.Property(nameof(Card.Title)).IsModified)
                    {
                        var oldTitle = entry.Property(nameof(Card.Title)).OriginalValue?.ToString();
                        var newTitle = entry.Property(nameof(Card.Title)).CurrentValue?.ToString();

                        logs.Add(new ActivityLog
                        {
                            BoardId = GetBoardIdForCard(entry, card.BoardListId),
                            CardId = card.Id,
                            ActionType = ActionType.CardTitleEdited,
                            Description = $"You renamed this card from '{oldTitle}' to '{newTitle}'",
                        });
                    }

                    if (entry.Property(nameof(Card.Priority)).IsModified)
                    {
                        var oldPriority = entry.Property(nameof(Card.Priority)).OriginalValue?.ToString();
                        var newPriority = entry.Property(nameof(Card.Priority)).CurrentValue?.ToString();

                        logs.Add(new ActivityLog
                        {
                            BoardId = GetBoardIdForCard(entry, card.BoardListId),
                            CardId = card.Id,
                            ActionType = ActionType.CardPriorityEdited,
                            Description = $"You changed card's priority from '{oldPriority}' to '{newPriority}'",
                        });
                    }

                    if (entry.Property(nameof(Card.Description)).IsModified)
                    {
                        logs.Add(new ActivityLog
                        {
                            BoardId = GetBoardIdForCard(entry, card.BoardListId),
                            CardId = card.Id,
                            ActionType = ActionType.CardDescriptionEdited,
                            Description = "You changed card's description.",
                        });
                    }

                    if (entry.Property(nameof(Card.BoardListId)).IsModified)
                    {
                        logs.Add(new ActivityLog
                        {
                            BoardId = GetBoardIdForCard(entry, card.BoardListId),
                            CardId = card.Id,
                            ActionType = ActionType.CardMoved,
                            Description = "You changed card's list.",
                        });
                    }

                    if (entry.Property(nameof(Card.DueDate)).IsModified)
                    {
                        var oldDueDate = entry.Property(nameof(Card.DueDate)).OriginalValue?.ToString();
                        var newDueDate = entry.Property(nameof(Card.DueDate)).CurrentValue?.ToString();

                        logs.Add(new ActivityLog
                        {
                            BoardId = GetBoardIdForCard(entry, card.BoardListId),
                            CardId = card.Id,
                            ActionType = ActionType.CardDueDateEdited,
                            Description = $"You changed card's due date from '{oldDueDate}' to '{newDueDate}'",
                        });
                    }
                }

                // BoardList logs
                if (entry.Entity is BoardList list)
                {
                    if (entry.Property(nameof(BoardList.Title)).IsModified)
                    {
                        var oldTitle = entry.Property(nameof(BoardList.Title)).OriginalValue?.ToString();
                        var newTitle = entry.Property(nameof(BoardList.Title)).CurrentValue?.ToString();

                        logs.Add(new ActivityLog
                        {
                            BoardId = list.BoardId,
                            BoardListId = list.Id,
                            ActionType = ActionType.ListTitleEdited,
                            Description = $"List renamed from '{oldTitle}' to '{newTitle}'"
                        });
                    }
                }

                // Board logs
                if (entry.Entity is Board board)
                {
                    if (entry.Property(nameof(Board.Title)).IsModified)
                    {
                        var oldTitle = entry.Property(nameof(Board.Title)).OriginalValue?.ToString();
                        var newTitle = entry.Property(nameof(Board.Title)).CurrentValue?.ToString();

                        logs.Add(new ActivityLog
                        {
                            BoardId = board.Id,
                            ActionType = ActionType.BoardRenamed,
                            Description = $"Board renamed from '{oldTitle}' to '{newTitle}'"
                        });
                    }
                }
            }

            // Added logs
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity is Card card)
                {
                    logs.Add(new ActivityLog { BoardId = GetBoardIdForCard(entry, card.BoardListId), BoardListId = card.BoardListId, CardId = card.Id, ActionType = ActionType.CardCreated, Description = $"Card '{card.Title}' was created." });
                }
                else if (entry.Entity is BoardList list)
                {
                    logs.Add(new ActivityLog { BoardId = list.BoardId, BoardListId = list.Id, ActionType = ActionType.ListCreated, Description = $"List '{list.Title}' was created." });
                }
                else if (entry.Entity is Board board)
                {
                    logs.Add(new ActivityLog { BoardId = board.Id, ActionType = ActionType.BoardCreated, Description = $"Board '{board.Title}' was created." });
                }
            }

            // Deleted logs
            if (entry.State == EntityState.Deleted)
            {
                if (entry.Entity is Card card)
                {
                    logs.Add(new ActivityLog { BoardId = GetBoardIdForCard(entry, card.BoardListId), CardId = card.Id, ActionType = ActionType.CardDeleted, Description = "Card was deleted." });
                }
                else if (entry.Entity is BoardList list)
                {
                    logs.Add(new ActivityLog { BoardId = list.BoardId, BoardListId = list.Id, ActionType = ActionType.ListDeleted, Description = "List was deleted." });
                }
                else if (entry.Entity is Board board)
                {
                    logs.Add(new ActivityLog { BoardId = board.Id, ActionType = ActionType.BoardDeleted, Description = "Board was deleted." });
                }
            }

            return logs;
        }
    }
}