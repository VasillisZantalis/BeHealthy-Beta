using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Doctor;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Controllers;

[Route("api/doctors")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
    }

    [HttpGet(Name = nameof(GetAllDoctors))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAllDoctors()
    {
        var doctors = await _doctorService.GetAllDoctorsAsync();

        return doctors is null ? NotFound() : Ok(doctors);
    }

    [HttpGet("{id:int}", Name = nameof(GetDoctorById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DoctorDto>> GetDoctorById(int id)
    {
        if (id < 0)
            return BadRequest();

        var doctor = await _doctorService.GetDoctorByIdAsync(id);

        return doctor is null ? NotFound() : Ok(doctor);
    }

    [HttpPost(Name = nameof(AddDoctor))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> AddDoctor(DoctorForCreationDto doctorDto)
    {
        if (doctorDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _doctorService.AddDoctorAsync(doctorDto);
        return Created();
    }

    [HttpPut("{id:int}", Name = nameof(UpdateDoctor))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> UpdateDoctor(int id, [FromBody] DoctorForUpdateDto doctorDto)
    {
        if (id < 0 || doctorDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _doctorService.UpdateDoctorAsync(doctorDto);

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = nameof(DeleteDoctor))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteDoctor(int id)
    {
        if (id < 0)
            return BadRequest();

        await _doctorService.DeleteDoctorAsync(id);

        return Ok();
    }
}
