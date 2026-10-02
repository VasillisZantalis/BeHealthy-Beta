using BeHealthy.API.Mapping;

namespace BeHealthy.API.Controllers;

[Route("api/settings")]
[ApiController]
public class AppSettingsController(IAppSettingsService appSettingsService) : ApiControllerBase
{
    /// <summary>Gets every application setting.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<AppSettingResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppSettingResponse>>> GetAll(CancellationToken cancellationToken)
        => Ok((await appSettingsService.GetAppSettingsAsync(cancellationToken)).Select(s => s.MapToDto()));

    /// <summary>Gets a single setting by key.</summary>
    [HttpGet("{key}")]
    [ProducesResponseType<AppSettingResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppSettingResponse>> GetByKey(string key, CancellationToken cancellationToken)
    {
        var setting = await appSettingsService.GetSettingByKeyAsync(key, cancellationToken);
        return setting is null ? NotFoundProblem("Setting", key) : Ok(setting.MapToDto());
    }

    /// <summary>Gets multiple settings by key in one call.</summary>
    [HttpPost("bulk")]
    [ProducesResponseType<IEnumerable<AppSettingResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppSettingResponse>>> GetBulk([FromBody] List<string> keys, CancellationToken cancellationToken)
        => Ok((await appSettingsService.GetMassAppSettingsAsync(keys, cancellationToken)).Select(s => s.MapToDto()));

    /// <summary>Updates the value of a setting.</summary>
    [HttpPut("{key}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string key, AppSettingUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (key != dto.Key)
            return Problem(
                detail: $"Route key '{key}' does not match body key '{dto.Key}'.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Key mismatch");

        var setting = await appSettingsService.GetSettingByKeyAsync(key, cancellationToken);
        if (setting is null)
            return NotFoundProblem("Setting", key);

        setting.Value = dto.Value;
        await appSettingsService.UpdateSettingAsync(setting, cancellationToken);
        return NoContent();
    }
}
