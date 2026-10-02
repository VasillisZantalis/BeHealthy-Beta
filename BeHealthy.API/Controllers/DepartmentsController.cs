using BeHealthy.Shared.Dtos.Department;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController(IDepartmentService departmentService) : ApiControllerBase
{
    /// <summary>Gets every department.</summary>
    [HttpGet]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<DepartmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetAll(CancellationToken cancellationToken)
        => Ok(await departmentService.GetAllDepartmentsAsync(cancellationToken));

    /// <summary>Gets a single department, including its doctors, nurses, patients, and rooms.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<DepartmentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var department = await departmentService.GetDepartmentByIdAsync(id, cancellationToken);
        return department is null ? NotFoundProblem("Department", id) : Ok(department);
    }

    /// <summary>Creates a new department.</summary>
    [HttpPost]
    [Authorize(Roles = RoleGroups.Administration)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(DepartmentCreateRequest dto, CancellationToken cancellationToken)
    {
        var response = await departmentService.AddDepartmentAsync(dto, cancellationToken);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing department.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = RoleGroups.Administration)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, DepartmentUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
        {
            return mismatch;
        }

        var response = await departmentService.UpdateDepartmentAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a department.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleGroups.Administration)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await departmentService.DeleteDepartmentAsync(id, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }
}
