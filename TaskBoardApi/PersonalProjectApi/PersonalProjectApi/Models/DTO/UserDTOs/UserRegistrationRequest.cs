using System.ComponentModel.DataAnnotations;

namespace PersonalProjectApi.Models.DTO.UserDTOs
{
    public class UserRegistrationRequest
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
