namespace PersonalProjectApi.Models.DTO.UserDTOs
{
    public sealed record UserLoginResponseDto(string AccessToken, DateTime ExpiresAtUtc);
}
