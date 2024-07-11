using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Prescription;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Controllers;

[Route("api/prescriptions")]
[ApiController]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionsController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService ?? throw new ArgumentNullException(nameof(prescriptionService));
    }

    [HttpGet(Name = nameof(GetAllPrescriptions))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetAllPrescriptions()
    {
        var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();

        return prescriptions is null ? NotFound() : Ok(prescriptions);
    }

    [HttpGet("{id:int}", Name = nameof(GetPrescriptionById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrescriptionDto>> GetPrescriptionById(int id)
    {
        if (id < 0)
            return BadRequest();

        var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);

        return prescription is null ? NotFound() : Ok(prescription);
    }

    [HttpPost(Name = nameof(AddPrescription))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> AddPrescription(PrescriptionForCreationDto prescriptionDto)
    {
        if (prescriptionDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _prescriptionService.AddPrescriptionAsync(prescriptionDto);
        return Created();
    }

    [HttpPut("{id:int}", Name = nameof(UpdatePrescription))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> UpdatePrescription(int id, [FromBody] PrescriptionForUpdateDto prescriptionDto)
    {
        if (id < 0 || prescriptionDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _prescriptionService.UpdatePrescriptionAsync(prescriptionDto);

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = nameof(DeletePrescription))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePrescription(int id)
    {
        if (id < 0)
            return BadRequest();

        await _prescriptionService.DeletePrescriptionAsync(id);

        return Ok();
    }
}
