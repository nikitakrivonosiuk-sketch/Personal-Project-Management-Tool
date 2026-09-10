namespace PersonalProjectApi.Models.DTO.BoardListsDto
{
    public class CreateListRequestDto
    {
        public Guid BoardId { get; set; }
        public string Title { get; set; }
    }
}
