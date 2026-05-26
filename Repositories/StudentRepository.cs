using Microsoft.EntityFrameworkCore;
using Student_directory_API.Data;
using Student_directory_API.Models;
using Student_directory_API.DTOs;

namespace Student_directory_API.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    // The Constructor: The database context is handed to the repository here
    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    // 1. GET ALL (With Searching, Sorting, and Pagination!)
    public async Task<(IEnumerable<Student> Items, int TotalCount)> GetAllAsync(StudentQueryParameters queryParams)
    {
        // Start by looking at the whole table, but don't download it yet.
        var query = _context.Students.AsQueryable();

        // A. Search Feature
        if (!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            var searchLower = queryParams.Search.ToLower();
            query = query.Where(s => s.Name.ToLower().Contains(searchLower) ||
                                     s.Email.ToLower().Contains(searchLower) ||
                                     s.PhoneNumber.Contains(searchLower));
        }

        // B. Ordering Feature
        if (queryParams.OrderBy?.ToLower() == "createddate")
        {
            query = queryParams.IsDescending ? query.OrderByDescending(s => s.CreatedDate) : query.OrderBy(s => s.CreatedDate);
        }
        else
        {
            query = queryParams.IsDescending ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name);
        }

        // C. Pagination Feature
        var totalCount = await query.CountAsync(); // Count how many total match the search
        var items = await query.Skip((queryParams.PageNumber - 1) * queryParams.PageSize) // Skip previous pages
                               .Take(queryParams.PageSize) // Take only what fits on this page
                               .ToListAsync(); // NOW finally download the data from Postgres

        return (items, totalCount);
    }

    // 2. GET BY ID
    public async Task<Student?> GetByIdAsync(Guid id) 
    {
        return await _context.Students.FindAsync(id);
    }

    // 3. GET BY EMAIL OR PHONE (Used to check for duplicates before creating)
    public async Task<Student?> GetByEmailOrPhoneAsync(string email, string phoneNumber)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.Email == email || s.PhoneNumber == phoneNumber);
    }

    // 4. CREATE
    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    // 5. UPDATE
    public async Task UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    // 6. DELETE
    public async Task DeleteAsync(Student student)
    {
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
    }
}