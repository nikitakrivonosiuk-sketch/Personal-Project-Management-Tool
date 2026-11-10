using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.DTO.UserDTOs;
using PersonalProjectApi.Services;
using PersonalProjectApi.Services.Authorization;
using System.Text.Json;

namespace PersonalProjectApi.Controllers
{
    [Route("api/Boards/{boardId}/Members")]
    [ApiController]
    [Authorize]
    public class BoardMemberController : ControllerBase
    {
        private readonly CurrentUserService _currentUserService;
        private readonly BoardMemberService _memberService;
        private readonly ILogger<BoardMemberController> _logger;
        public BoardMemberController(CurrentUserService currentUserService,
            BoardMemberService boardMemberService,
            ILogger<BoardMemberController> logger)
        {
            _currentUserService = currentUserService;
            _memberService = boardMemberService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBoardMembersAsync([FromRoute] Guid boardId)
        {
            _logger.LogInformation($"{nameof(GetAllBoardMembersAsync)} received request: {boardId}. From userId: {_currentUserService.UserId}");

            var result = await _memberService.GetAllBoardUsersAsync(boardId, _currentUserService.UserId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddBoardMember([FromRoute] Guid boardId,[FromBody] AddBoardMemberRequestDto requestDto)
        {
            _logger.LogInformation($"{nameof(AddBoardMember)} received request: {JsonSerializer.Serialize(requestDto)}.");

            var user = await _memberService.AddUserToBoard(requestDto, boardId, _currentUserService.UserId);

            return Ok(user);
        }

        [HttpPut("{memberUserId:guid}")]
        public async Task<IActionResult> UpdateBoardMember([FromRoute] Guid boardId, [FromRoute] Guid memberUserId, [FromBody] UpdateBoardMemberRequestDto requestDto)
        {
            _logger.LogInformation($"{nameof(UpdateBoardMember)} received request: {JsonSerializer.Serialize(requestDto)}. From UserID: {_currentUserService.UserId}");

            var updatedMember = await _memberService.UpdateBoardMemberAsync(boardId, memberUserId, _currentUserService.UserId, requestDto);

            return Ok(updatedMember);
        }

        [HttpDelete("{memberUserId:guid}")]
        public async Task<IActionResult> DeleteBoardMember([FromRoute] Guid boardId, [FromRoute] Guid memberUserId)
        {
            _logger.LogInformation($"{nameof(DeleteBoardMember)} received request: {boardId} and {memberUserId}. From userID: {_currentUserService.UserId}");

            var result = await _memberService.DeleteBoardMemberAsync(boardId, memberUserId, _currentUserService.UserId);
            return Ok(result);
        }
    }
}
