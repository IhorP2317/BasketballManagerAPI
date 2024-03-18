using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/awards")]
    public class AwardController:ControllerBase {
        private readonly IAwardService _awardService;

        public AwardController(IAwardService awardService)
        {
            _awardService = awardService;
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("{id:guid}")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<IActionResult> UpdateAwardAsync([FromRoute] Guid id,
            [FromBody] AwardUpdateDto awardUpdateDto, CancellationToken cancellationToken) {
            await _awardService.UpdateAwardAsync(id, awardUpdateDto, cancellationToken);
            return NoContent();

        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPatch("{id}/avatar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePlayerAvatarAsync([FromRoute] Guid id,
            [FromForm] IFormFile picture,
            CancellationToken cancellationToken) {
            await _awardService.UpdateAwardAvatarAsync(id, picture, cancellationToken);
            return NoContent();

        }

        [HttpGet("{id}/avatar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadPlayerAvatarAsync([FromRoute] Guid id,
            CancellationToken cancellationToken) {
            var fileDto = await _awardService.DownloadAwardAvatarAsync(id, cancellationToken);
            return File(fileDto.Content, fileDto.MimeType, fileDto.FileName);
        }

    }
}
