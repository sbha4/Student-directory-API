using Student_directory_API.DTOs;

namespace Student_directory_API.Services;

public interface IStudentService
{
    Task<PagedResult<StudentResponseDto>> GetAllStudentsAsync(StudentQueryParameters queryParams);
    Task<StudentResponseDto?> GetStudentByIdAsync(Guid id);
    Task<StudentResponseDto> CreateStudentAsync(StudentCreateDto dto);
    Task<bool> UpdateStudentAsync(Guid id, StudentUpdateDto dto);
    Task<bool> DeleteStudentAsync(Guid id);
}