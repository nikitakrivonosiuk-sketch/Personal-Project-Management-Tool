using PersonalProjectApi.Models.Enums;

namespace PersonalProjectApi.Models.Domain
{
    public class BoardsMembers : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid BoardId { get; set; }
        public UserRoles userRole { get; set; } = new UserRoles();

        public ICollection<Board>? Boards { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
