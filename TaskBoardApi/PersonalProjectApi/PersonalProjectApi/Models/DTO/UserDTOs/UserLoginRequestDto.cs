using System.ComponentModel.DataAnnotations;

namespace PersonalProjectApi.Models.DTO.UserDTOs
{
    public class UserLoginRequestDto
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
