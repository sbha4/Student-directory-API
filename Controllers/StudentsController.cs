using Microsoft.AspNetCore.Mvc;
using Student_directory_API.DTOs;
using Student_directory_API.Services;

namespace Student_directory_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    // The Constructor: Hands the Service (the brain) to the Controller
    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    // 1. GET ALL
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] StudentQueryParameters queryParams)
    {
        var result = await _service.GetAllStudentsAsync(queryParams);
        return Ok(result); // Returns a 200 Success status code with the data
    }

    // 2. GET BY ID
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var student = await _service.GetStudentByIdAsync(id);
        if (student == null) return NotFound("Student not found."); // Returns a 404 Error
        
        return Ok(student);
    }

    // 3. CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentCreateDto dto)
    {
        try
        {
            var createdStudent = await _service.CreateStudentAsync(dto);
            // Returns a 201 Created status code, and tells the user where to find the new student
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
        }
        catch (InvalidOperationException ex)
        {
            // If the Service throws our "Email already exists" error, return a 400 Bad Request
            return BadRequest(ex.Message);
        }
    }

    // 4. UPDATE
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StudentUpdateDto dto)
    {
        var updated = await _service.UpdateStudentAsync(id, dto);
        if (!updated) return NotFound("Student not found.");

        return NoContent(); // Returns a 204 status code (Success, but no data to send back)
    }

    // 5. DELETE
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteStudentAsync(id);
        if (!deleted) return NotFound("Student not found.");

        return NoContent();
    }
}