using UsersTeachers.Repositories;

namespace UsersTeachers.Services;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ITeacherRepository Teachers { get; }
    IStudentRepository Students { get; }

    Task<int> SaveChangesAsync();
}
