using Microsoft.EntityFrameworkCore;

namespace UsersTeachers.Data;

public class UsersTeachersDbContext : DbContext
{
    public UsersTeachersDbContext(){

    }

    public UsersTeachersDbContext(DbContextOptions<UsersTeachersDbContext> options) : base(options)
    {

    }

    public virtual DbSet<Teacher> Teachers {get; set;}
    public virtual DbSet<Student> Students {get; set;}
    public virtual DbSet<User> Users {get; set;}
    public virtual DbSet<Course> Courses {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity => 
        {
            entity.ToTable("STUDENTS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Institution).HasMaxLength(50).HasColumnName("INSTITUTION");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50).HasColumnName("PHONE_NUMBER");
            entity.HasOne(d => d.User).WithOne(p => p.Student).HasForeignKey<Student>(d => d.UserId).HasConstraintName("FK_STUDENTS_USERS");
            entity.HasMany(s => s.Courses).WithMany(c => c.Students).UsingEntity("STUDENTS_COURSES");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("TEACHERS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Institution).HasMaxLength(50).HasColumnName("INSTITUTION");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50).HasColumnName("PHONE_NUMBER");
            entity.HasOne(d => d.User).WithOne(p => p.Teacher).HasForeignKey<Teacher>(d => d.UserId).HasConstraintName("FK_TEACHERS_USERS");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("COURSES");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TeacherId).HasColumnName("TEACHER_ID");
            entity.Property(e => e.Description).HasMaxLength(50).HasColumnName("DESCRIPTION");
            entity.HasOne(c => c.Teacher).WithMany(t => t.Courses).HasForeignKey(c => c.TeacherId).HasConstraintName("FK_TEACHERS_COURSES");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USERS");
            entity.HasIndex(e => e.Lastname, "IX_LASTNAME");
            entity.HasIndex(e => e.Username, "UQ_USERNAME").IsUnique();
            entity.HasIndex(e => e.Email, "UQ_EMAIL").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("USERNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(60)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("FIRSTNAME");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("LASTNAME");
            entity.Property(e => e.UserRole)
                .HasColumnName("USER_ROLE")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();
        });

 }




}