using Autogestion.Application.Interfaces;
using Autogestion.Application.UseCases.Courses;
using Autogestion.Application.UseCases.Exams;
using Autogestion.Application.UseCases.Plans;
using Autogestion.Application.UseCases.Status;
using Autogestion.Infrastructure.Data;
using Autogestion.Infrastructure.Data.Seed;
using Autogestion.Infrastructure.Services;
using Autogestion.Infrastructure.UseCases.Courses;
using Autogestion.Infrastructure.UseCases.Exams;
using Autogestion.Infrastructure.UseCases.Plans;
using Autogestion.Infrastructure.UseCases.Status;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<IGetStudentPlanQuery, GetStudentPlanQuery>();
builder.Services.AddScoped<IGetStudentStatusQuery, GetStudentStatusQuery>();
builder.Services.AddScoped<IGetExamCallsQuery, GetExamCallsQuery>();
builder.Services.AddScoped<IEnrollExamCommand, EnrollExamCommand>();
builder.Services.AddScoped<IGetCourseOffersQuery, GetCourseOffersQuery>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalDev", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    if (builder.Configuration.GetValue<bool>("SeedData"))
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        SeedData.EnsureSeedData(dbContext);
    }
}

app.UseCors("AllowLocalDev");
app.UseHttpsRedirection();

// Se conecta a la base local; no aplica seed automatico.

app.MapGet("/students/me/plan", async (IPlanService planService, CancellationToken cancellationToken) =>
{
    const int demoStudentId = 1;
    var plan = await planService.GetPlanForStudentAsync(demoStudentId, cancellationToken);
    return plan is null ? Results.NotFound() : Results.Ok(plan);
});

app.Run();
