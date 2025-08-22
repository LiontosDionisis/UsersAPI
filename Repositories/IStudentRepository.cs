using UsersTeachers.Data;

namespace UsersTeachers.Repositories;

public interface IStudentRepository
{
    Task<List<Course>> GetStudentCourseAsync(int id);
    Task<Student?> GetByPhoneNumber(string? phoneNumber);
    Task<List<User>> GetAllUsersStudentAsync();
    Task<List<User>> GetAllUsersStudentAsync(int pageNumber, int pageSize);
    Task<User?> GetStudentByUsernameAsync(string username);
}