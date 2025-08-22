using UsersTeachers.Data;

namespace UsersTeachers.Repositories;

public interface ICourseRepository
{
    Task<List<Student>> GetCourseStudentAsync(int id);
    Task<Teacher?> GetCourseTeacherAsync(int id);
}