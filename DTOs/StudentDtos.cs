namespace Student_directory_API.DTOs;

// 1. Response DTO (Notice: No Password field!)
public record StudentResponseDto(Guid Id, string Name, string PhoneNumber, string Email, DateTime CreatedDate);

// 2. Request DTOs (For when a user is creating or updating a student)
public record StudentCreateDto(string Name, string PhoneNumber, string Email, string Password);
public record StudentUpdateDto(string Name, string PhoneNumber, string Email);

// 3. Pagination & Filtering DTO (Handles your Search and Sorting requirements)
public class StudentQueryParameters
{
    public string? Search { get; set; }
    public string? OrderBy { get; set; } = "Name"; 
    public bool IsDescending { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

// 4. Paginated Response Wrapper (Formats the pages correctly)
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}