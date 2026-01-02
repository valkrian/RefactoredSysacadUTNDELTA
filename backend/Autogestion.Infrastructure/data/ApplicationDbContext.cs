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
        
    }
}