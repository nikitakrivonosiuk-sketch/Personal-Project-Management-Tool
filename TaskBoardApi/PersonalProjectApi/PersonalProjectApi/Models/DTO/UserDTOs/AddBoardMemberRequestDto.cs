using PersonalProjectApi.Models.Enums;

namespace PersonalProjectApi.Models.DTO.UserDTOs
{
    public class AddBoardMemberRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public UserLevelAccess UserLevel { get; set; }
    }
}
