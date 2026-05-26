using Microsoft.EntityFrameworkCore;
using Student_directory_API.Data;
using Student_directory_API.Repositories;
using Student_directory_API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 1. ADD SWAGGER UI BACK
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Dependency Injection
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // 4. TURN SWAGGER ON
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();