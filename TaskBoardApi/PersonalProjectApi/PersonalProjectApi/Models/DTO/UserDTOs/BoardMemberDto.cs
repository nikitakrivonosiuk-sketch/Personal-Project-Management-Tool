using PersonalProjectApi.Models.Enums;

namespace PersonalProjectApi.Models.DTO.UserDTOs
{
    public class BoardMemberDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public UserLevelAccess UserLevel { get; set; }
    }
}
