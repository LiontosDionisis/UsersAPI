using UsersTeachers.Data;
using UsersTeachers.Repositories;

namespace UsersTeachers.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly UsersTeachersDbContext _context;

    public IUserRepository Users { get; }
    public ITeacherRepository Teachers { get; }
    public IStudentRepository Students { get; }

    public UnitOfWork(UsersTeachersDbContext context, IUserRepository users, ITeacherRepository teachers, IStudentRepository students)
    {
        _context = context;
        Users = users;
        Teachers = teachers;
        Students = students;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
