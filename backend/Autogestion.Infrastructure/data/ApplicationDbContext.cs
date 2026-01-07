using Microsoft.EntityFrameworkCore;
using Autogestion.Domain.Entities;

namespace Autogestion.Infrastructure.Data;

// Hereda de DbContext (clase base de EF Core)
public class ApplicationDbContext : DbContext
{
    // Constructor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // base(options): Pasa las opciones al constructor de DbContext
    }
    
    // DbSet: Representa una tabla en la base de datos
    // Cada DbSet mapea una entidad a una tabla
    
    public DbSet<Plan> Plans => Set<Plan>();

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<CourseEnrollment> CourseEnrollments => Set<CourseEnrollment>();
    public DbSet<ExamCall> ExamCalls => Set<ExamCall>();
    public DbSet<ExamEnrollment> ExamEnrollments => Set<ExamEnrollment>();
    public DbSet<Prerequisite> Prerequisites => Set<Prerequisite>();

    public DbSet<ExamResult> ExamResults => Set<ExamResult>();
    
    // OnModelCreating: Método donde configuramos el modelo de datos
    // Se ejecuta cuando EF Core construye el modelo
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        
        
        modelBuilder.Entity<Plan>(entity =>
        {
            
            
            
            entity.HasKey(e => e.Id);
            
            
            
            
            entity.HasIndex(e => e.Name);
            // HasIndex: Crea un índice en la columna Name
            // Útil para búsquedas por nombre
            
            // Propiedades
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Career)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.YearVersion)
                .IsRequired();
        });
        
        // Configuración de Student
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            
            entity.HasIndex(e => e.Legajo)
                .IsUnique();
            
            entity.HasIndex(e => e.Email)
                .IsUnique();
            
            
            entity.Property(e => e.Legajo)
                .IsRequired()
                .HasMaxLength(20);
            
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);
            
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();
            
            
            entity.HasOne(e => e.Plan)
                .WithMany(p => p.Students)
                .HasForeignKey(e => e.PlanId)
                .OnDelete(DeleteBehavior.Restrict);
            
        });
        
        // Configuración de Subject
        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasIndex(e => e.Code)
                .IsUnique();
            
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(20);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Year)
                .IsRequired();
            
            entity.Property(e => e.Term)
                .IsRequired();
        });
        
        // Relación muchos-a-muchos: Plan <-> Subject
        // EF Core necesita una tabla intermedia (PlanSubject)
        modelBuilder.Entity<Plan>()
    .HasMany(p => p.Subjects)
    .WithMany(s => s.Plans)
    .UsingEntity<Dictionary<string, object>>(
        "PlanSubject",
        j => j.HasOne<Subject>().WithMany().HasForeignKey("SubjectId"),
        j => j.HasOne<Plan>().WithMany().HasForeignKey("PlanId")
    );

modelBuilder.Entity<CourseEnrollment>(entity =>
{
    entity.HasKey(e => new { e.StudentId, e.SubjectId, e.Period });

    entity.Property(e => e.Period)
        .IsRequired()
        .HasMaxLength(20);

    entity.Property(e => e.Status)
        .IsRequired();

    entity.HasOne(e => e.Student)
        .WithMany(s => s.CourseEnrollments)
        .HasForeignKey(e => e.StudentId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(e => e.Subject)
        .WithMany(s => s.CourseEnrollments)
        .HasForeignKey(e => e.SubjectId)
        .OnDelete(DeleteBehavior.Restrict);
});

modelBuilder.Entity<ExamCall>(entity =>
{
    entity.HasKey(e => e.Id);

    entity.Property(e => e.StartsAt)
        .IsRequired();

    entity.Property(e => e.EndsAt)
        .IsRequired();

    entity.Property(e => e.Capacity)
        .IsRequired();

    entity.HasOne(e => e.Subject)
        .WithMany(s => s.ExamCalls)
        .HasForeignKey(e => e.SubjectId)
        .OnDelete(DeleteBehavior.Restrict);
});

modelBuilder.Entity<ExamEnrollment>(entity =>
{
    entity.HasKey(e => new { e.StudentId, e.ExamCallId });

    entity.Property(e => e.EnrolledAt)
        .IsRequired();

    entity.HasOne(e => e.Student)
        .WithMany(s => s.ExamEnrollments)
        .HasForeignKey(e => e.StudentId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(e => e.ExamCall)
        .WithMany(ec => ec.Enrollments)
        .HasForeignKey(e => e.ExamCallId)
        .OnDelete(DeleteBehavior.Restrict);
});

modelBuilder.Entity<ExamResult>(entity =>
{
    entity.HasKey(e => new { e.StudentId, e.SubjectId });

    entity.Property(e => e.Date)
        .IsRequired();

    entity.Property(e => e.Grade)
        .IsRequired();

    entity.HasOne(e => e.Student)
        .WithMany(s => s.ExamResults)
        .HasForeignKey(e => e.StudentId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(e => e.Subject)
        .WithMany(s => s.ExamResults)
        .HasForeignKey(e => e.SubjectId)
        .OnDelete(DeleteBehavior.Restrict);
});

modelBuilder.Entity<Prerequisite>(entity =>
{
    entity.HasKey(e => new { e.SubjectId, e.RequiresSubjectId, e.Type, e.MinimumStatus });

    entity.Property(e => e.SubjectId)
        .IsRequired();

    entity.Property(e => e.RequiresSubjectId)
        .IsRequired();

    entity.Property(e => e.Type)
        .IsRequired();

    entity.Property(e => e.MinimumStatus)
        .IsRequired();

    entity.HasOne(e => e.Subject)
        .WithMany(s => s.Prerequisites)
        .HasForeignKey(e => e.SubjectId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(e => e.RequiresSubject)
        .WithMany(s => s.RequiredBy)
        .HasForeignKey(e => e.RequiresSubjectId)
        .OnDelete(DeleteBehavior.Restrict);
});
    }
}
