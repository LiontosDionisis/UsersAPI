using Microsoft.EntityFrameworkCore;
using UsersTeachers.Data;
using UsersTeachers.Models;
using UsersTeachers.Repositories;

namespace UsersTeachers.Repositories;

public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
{

    public TeacherRepository(UsersTeachersDbContext context) : base(context)
    {
    }
    public async Task<List<User>> GetAllUsersTeachersAsync()
    {
        var usersWithTeacherRole = await _context.Users.Where(u => u.UserRole == UserRole.Teacher).Include(u => u.Teacher).ToListAsync();

        return usersWithTeacherRole;
    }

    public async Task<List<User>> GetAllUsersTeachersAsync(int pageNumber, int pageSize)
    {
        int skip = pageNumber * pageSize;
        var usersWithTeacherRole = await _context.Users.Where(u => u.UserRole == UserRole.Teacher).Include(u => u.Teacher).Skip(skip).Take(pageSize).ToListAsync();

        return usersWithTeacherRole;
    }

    public async Task<Teacher?> GetByPhoneNumber(string? phoneNumber)
    {
        return await _context.Teachers.Where(s => s.PhoneNumber == phoneNumber).FirstOrDefaultAsync()!;
    }

    public async Task<User?> GetTeacherByUsernameAsync(string username)
    {
        var userTeacher = await _context.Users.Where(u => u.Username == username && u.UserRole == UserRole.Teacher).SingleOrDefaultAsync();

        return userTeacher;
    }

    public async Task<List<Course>> GetTeacherCourseAsync(int id)
    {
        List<Course> courses;
        courses = await _context.Teachers.Where(t => t.Id == id).SelectMany(t => t.Courses).ToListAsync();

        return courses;
    }
}