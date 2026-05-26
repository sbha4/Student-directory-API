using Student_directory_API.DTOs;
using Student_directory_API.Models;
using Student_directory_API.Repositories;

namespace Student_directory_API.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    // The Constructor: The Service is handed the Repository so it can talk to the database
    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<StudentResponseDto>> GetAllStudentsAsync(StudentQueryParameters queryParams)
    {
        // 1. Ask the repository for the data
        var (items, totalCount) = await _repository.GetAllAsync(queryParams);
        
        // 2. Filter out the passwords! Convert every 'Student' into a safe 'StudentResponseDto'
        var dtos = items.Select(s => new StudentResponseDto(s.Id, s.Name, s.PhoneNumber, s.Email, s.CreatedDate));

        // 3. Package it up nicely with the pagination details
        return new PagedResult<StudentResponseDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = queryParams.PageNumber,
            PageSize = queryParams.PageSize
        };
    }

    public async Task<StudentResponseDto?> GetStudentByIdAsync(Guid id)
    {
        var student = await _repository.GetByIdAsync(id);
        if (student == null) return null;

        // Strip the password before returning!
        return new StudentResponseDto(student.Id, student.Name, student.PhoneNumber, student.Email, student.CreatedDate);
    }

    public async Task<StudentResponseDto> CreateStudentAsync(StudentCreateDto dto)
    {
        // BUSINESS LOGIC: Check for duplicates before creating
        var existing = await _repository.GetByEmailOrPhoneAsync(dto.Email, dto.PhoneNumber);
        if (existing != null)
        {
            // If it exists, crash the process and send this exact error message back
            throw new InvalidOperationException("Email or Phone Number already exists.");
        }

        // Convert the Request DTO into a real Student Entity for the database
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Password = dto.Password, 
            CreatedDate = DateTime.UtcNow // Automatically set the exact time they are created
        };

        // Tell the repository to save it
        await _repository.AddAsync(student);

        // Return the safe version without the password
        return new StudentResponseDto(student.Id, student.Name, student.PhoneNumber, student.Email, student.CreatedDate);
    }

    public async Task<bool> UpdateStudentAsync(Guid id, StudentUpdateDto dto)
    {
        var student = await _repository.GetByIdAsync(id);
        if (student == null) return false;

        // Update the fields
        student.Name = dto.Name;
        student.PhoneNumber = dto.PhoneNumber;
        student.Email = dto.Email;

        await _repository.UpdateAsync(student);
        return true;
    }

    public async Task<bool> DeleteStudentAsync(Guid id)
    {
        var student = await _repository.GetByIdAsync(id);
        if (student == null) return false;

        await _repository.DeleteAsync(student);
        return true;
    }
}