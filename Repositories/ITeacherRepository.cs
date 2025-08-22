using UsersTeachers.Data;

namespace UsersTeachers.Repositories;

public interface ITeacherRepository
{
    Task<List<Course>> GetTeacherCourseAsync(int id);
    Task<Teacher?> GetByPhoneNumber(string? phoneNumber);
    Task<List<User>> GetAllUsersTeachersAsync();
    Task<List<User>> GetAllUsersTeachersAsync(int pageNumber, int pageSize);
    Task<User?> GetTeacherByUsernameAsync(string username);
}