using BeHealthy.Shared.Dtos.Department;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController(IDepartmentService departmentService) : ApiControllerBase
{
    /// <summary>Gets every department.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<DepartmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetAll()
        => Ok(await departmentService.GetAllDepartmentsAsync());

    /// <summary>Gets a single department, including its doctors, nurses, patients, and rooms.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<DepartmentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentResponse>> GetById(int id)
    {
        var department = await departmentService.GetDepartmentByIdAsync(id);
        return department is null ? NotFoundProblem("Department", id) : Ok(department);
    }

    /// <summary>Creates a new department.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(DepartmentCreateRequest dto)
    {
        var response = await departmentService.AddDepartmentAsync(dto);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing department.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, DepartmentUpdateRequest dto)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await departmentService.UpdateDepartmentAsync(dto);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a department.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await departmentService.DeleteDepartmentAsync(id);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }
}
