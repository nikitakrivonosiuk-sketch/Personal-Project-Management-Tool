using PersonalProjectApi.Models.Enums;

namespace PersonalProjectApi.Models.DTO.UserDTOs
{
    public class UpdateBoardMemberRequestDto
    {
        public UserLevelAccess LevelAccess { get; set; }
    }
}
