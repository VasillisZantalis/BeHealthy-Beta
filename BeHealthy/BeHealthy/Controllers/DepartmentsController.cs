using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Controllers;

[Route("api/departments")]
[ApiController]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
    }

    [HttpGet(Name = nameof(GetAllDepartments))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();

        return departments is null ? NotFound() : Ok(departments);
    }

    [HttpGet("{id:int}", Name = nameof(GetDepartmentById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int id)
    {
        if (id < 0)
            return BadRequest();

        var department = await _departmentService.GetDepartmentByIdAsync(id);

        return department is null ? NotFound() : Ok(department);
    }

    [HttpPost(Name = nameof(AddDepartment))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> AddDepartment(DepartmentForCreationDto departmentDto)
    {
        if (departmentDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _departmentService.AddDepartmentAsync(departmentDto);
        return Created();
    }

    [HttpPut("{id:int}", Name = nameof(UpdateDepartment))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> UpdateDepartment(int id, [FromBody] DepartmentForUpdateDto departmentDto)
    {
        if (id < 0 || departmentDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _departmentService.UpdateDepartmentAsync(departmentDto);

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = nameof(DeleteDepartment))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteDepartment(int id)
    {
        if (id < 0)
            return BadRequest();

        await _departmentService.DeleteDepartmentAsync(id);

        return Ok();
    }
}
