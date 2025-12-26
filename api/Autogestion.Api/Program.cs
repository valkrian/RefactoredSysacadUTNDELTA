using Autogestion.Application.Interfaces;
using Autogestion.Infrastructure.Data;
using Autogestion.Infrastructure.Data.Seed;
using Autogestion.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("AutogestionDb"));

builder.Services.AddScoped<IPlanService, PlanService>();
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
}

app.UseCors("AllowLocalDev");
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeedData.EnsureSeedData(dbContext);
}

app.MapGet("/students/me/plan", async (IPlanService planService, CancellationToken cancellationToken) =>
{
    const int demoStudentId = 1;
    var plan = await planService.GetPlanForStudentAsync(demoStudentId, cancellationToken);
    return plan is null ? Results.NotFound() : Results.Ok(plan);
});

app.Run();
