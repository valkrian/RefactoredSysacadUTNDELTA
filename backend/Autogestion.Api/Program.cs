using Autogestion.Application.DTOs;
using Autogestion.Application.UseCases.Courses;
using Autogestion.Application.UseCases.Exams;
using Autogestion.Application.UseCases.Plans;
using Autogestion.Application.UseCases.Status;
using Autogestion.Infrastructure.Data;
using Autogestion.Infrastructure.Data.Seed;
using Autogestion.Infrastructure.UseCases.Courses;
using Autogestion.Infrastructure.UseCases.Exams;
using Autogestion.Infrastructure.UseCases.Plans;
using Autogestion.Infrastructure.UseCases.Status;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IGetStudentPlanQuery, GetStudentPlanQuery>();
builder.Services.AddScoped<IGetStudentStatusQuery, GetStudentStatusQuery>();
builder.Services.AddScoped<IGetExamCallsQuery, GetExamCallsQuery>();
builder.Services.AddScoped<IEnrollExamCommand, EnrollExamCommand>();
builder.Services.AddScoped<IGetExamEnrollmentsQuery, GetExamEnrollmentsQuery>();
builder.Services.AddScoped<IGetCourseOffersQuery, GetCourseOffersQuery>();
builder.Services.AddScoped<IEnrollCourseCommand, EnrollCourseCommand>();
builder.Services.AddScoped<IGetCourseEnrollmentsQuery, GetCourseEnrollmentsQuery>();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-secret-key-change-me";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "Autogestion.Api";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "Autogestion.Web";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>()
        ?? new[] { "http://localhost:4200" };

    options.AddPolicy("AllowLocalDev", policy =>
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod());
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
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/auth/login", async (ApplicationDbContext dbContext, LoginRequest request, CancellationToken ct) =>
{
    var student = await dbContext.Students
        .FirstOrDefaultAsync(s => s.Email == request.Email && s.PasswordHash == request.Password, ct);

    if (student is null)
    {
        return Results.Unauthorized();
    }

    var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, student.Id.ToString()),
        new(ClaimTypes.Name, student.FullName),
        new(ClaimTypes.Email, student.Email)
    };

    var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: jwtIssuer,
        audience: jwtAudience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(8),
        signingCredentials: creds);

    var response = new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token), student.Id, student.FullName, student.Email);
    return Results.Ok(response);
});

app.MapGet("/me", async (ApplicationDbContext dbContext, ClaimsPrincipal user, CancellationToken ct) =>
{
    var studentId = GetStudentId(user);
    if (studentId is null)
    {
        return Results.Unauthorized();
    }

    var student = await dbContext.Students
        .AsNoTracking()
        .FirstOrDefaultAsync(s => s.Id == studentId.Value, ct);

    return student is null
        ? Results.NotFound()
        : Results.Ok(new MeResponse(student.Id, student.FullName, student.Email, student.PlanId));
}).RequireAuthorization();

app.MapGet("/students/me/plan", async (IGetStudentPlanQuery query, ClaimsPrincipal user, CancellationToken ct) =>
{
    var studentId = GetStudentId(user);
    if (studentId is null)
    {
        return Results.Unauthorized();
    }

    var result = await query.ExecuteAsync(studentId.Value, ct);
    return result.Success ? Results.Ok(result.Value) : Results.NotFound(new { error = result.Error });
}).RequireAuthorization();

app.MapGet("/students/me/status", async (IGetStudentStatusQuery query, ClaimsPrincipal user, CancellationToken ct) =>
{
    var studentId = GetStudentId(user);
    if (studentId is null)
    {
        return Results.Unauthorized();
    }

    var result = await query.ExecuteAsync(studentId.Value, ct);
    return result.Success ? Results.Ok(result.Value) : Results.NotFound(new { error = result.Error });
}).RequireAuthorization();

app.MapGet("/exam-calls", async (IGetExamCallsQuery query, DateTime? from, DateTime? to, int? subjectId, CancellationToken ct) =>
{
    var result = await query.ExecuteAsync(from, to, subjectId, ct);
    return Results.Ok(result.Value);
}).RequireAuthorization();

app.MapPost("/students/me/exam-enrollments", async (IEnrollExamCommand command, ClaimsPrincipal user, EnrollExamRequest request, CancellationToken ct) =>
{
    var studentId = GetStudentId(user);
    if (studentId is null)
    {
        return Results.Unauthorized();
    }

    var result = await command.ExecuteAsync(studentId.Value, request.ExamCallId, ct);
    return MapCommandResult(result);
}).RequireAuthorization();

app.MapGet("/students/me/exam-enrollments", async (IGetExamEnrollmentsQuery query, ClaimsPrincipal user, CancellationToken ct) =>
{
    var studentId = GetStudentId(user);
    if (studentId is null)
    {
        return Results.Unauthorized();
    }

    var result = await query.ExecuteAsync(studentId.Value, ct);
    return Results.Ok(result.Value);
}).RequireAuthorization();

app.MapGet("/course-offers", async (IGetCourseOffersQuery query, string? period, CancellationToken ct) =>
{
    var result = await query.ExecuteAsync(period, ct);
    return Results.Ok(result.Value);
}).RequireAuthorization();

app.MapPost("/students/me/course-enrollments", async (IEnrollCourseCommand command, ClaimsPrincipal user, EnrollCourseRequest request, CancellationToken ct) =>
{
    var studentId = GetStudentId(user);
    if (studentId is null)
    {
        return Results.Unauthorized();
    }

    var result = await command.ExecuteAsync(studentId.Value, request.SubjectId, request.Period, ct);
    return MapCommandResult(result);
}).RequireAuthorization();

app.MapGet("/students/me/course-enrollments", async (IGetCourseEnrollmentsQuery query, ClaimsPrincipal user, CancellationToken ct) =>
{
    var studentId = GetStudentId(user);
    if (studentId is null)
    {
        return Results.Unauthorized();
    }

    var result = await query.ExecuteAsync(studentId.Value, ct);
    return Results.Ok(result.Value);
}).RequireAuthorization();

app.MapPut("/me/profile", async (ApplicationDbContext db, ClaimsPrincipal user, UpdateProfileRequest req, CancellationToken ct) =>
{
    var studentId = GetStudentId(user);
    if (studentId is null)
        return Results.Unauthorized();

    var student = await db.Students.FindAsync([studentId.Value], ct);
    if (student is null)
        return Results.NotFound();

    student.FullName = req.FullName;
    student.Email = req.Email;

    await db.SaveChangesAsync(ct);
    return Results.Ok();
}).RequireAuthorization();


app.MapPut("/me/password", async (ApplicationDbContext db, ClaimsPrincipal user, ChangePasswordRequest req, CancellationToken ct) =>
{
    var studentId = GetStudentId(user);
    if (studentId is null)
        return Results.Unauthorized();

    var student = await db.Students.FindAsync([studentId.Value], ct);
    if (student is null)
        return Results.NotFound();

    if (student.PasswordHash != req.CurrentPassword)
        return Results.BadRequest(new { error = "Password incorrecta" });

    student.PasswordHash = req.NewPassword;
    await db.SaveChangesAsync(ct);

    return Results.Ok();
}).RequireAuthorization();




app.MapGet("/me/profile", async (ApplicationDbContext dbContext, ClaimsPrincipal user, CancellationToken ct) =>
{
    var studentId = GetStudentId(user);
    if (studentId is null)
    {
        return Results.Unauthorized();
    }

    var student = await dbContext.Students
        .AsNoTracking()
        .FirstOrDefaultAsync(s => s.Id == studentId.Value, ct);

    if (student is null)
    {
        return Results.NotFound();
    }

    var profileDto = new ProfileDto
    {
        StudentId = student.Id,
        FullName = student.FullName,
        Email = student.Email,
        Legajo = student.Legajo
    };

    return Results.Ok(profileDto);
}).RequireAuthorization();

static int? GetStudentId(ClaimsPrincipal user)
{
    var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    return int.TryParse(claim, out var studentId) ? studentId : null;
}

static IResult MapCommandResult(Autogestion.Application.Shared.Result result)
{
    if (result.Success)
    {
        return Results.Ok();
    }

    if (result.Error?.Contains("Prerequisites", StringComparison.OrdinalIgnoreCase) == true)
    {
        return Results.UnprocessableEntity(new { error = result.Error });
    }

    return Results.Conflict(new { error = result.Error });
}

app.Run();

record LoginRequest(string Email, string Password);
record LoginResponse(string Token, int StudentId, string FullName, string Email);
record MeResponse(int StudentId, string FullName, string Email, int PlanId);
record EnrollExamRequest(int ExamCallId);
record EnrollCourseRequest(int SubjectId, string Period);
record UpdateProfileRequest(string FullName, string Email);
record ChangePasswordRequest(string CurrentPassword, string NewPassword);
