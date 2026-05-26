using Student_directory_API.Models;
using Student_directory_API.DTOs;

namespace Student_directory_API.Repositories;

public interface IStudentRepository
{
    Task<(IEnumerable<Student> Items, int TotalCount)> GetAllAsync(StudentQueryParameters queryParams);
    Task<Student?> GetByIdAsync(Guid id);
    Task<Student?> GetByEmailOrPhoneAsync(string email, string phoneNumber);
    
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(Student student);
}