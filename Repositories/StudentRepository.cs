using Microsoft.EntityFrameworkCore;
using UsersTeachers.Data;
using UsersTeachers.Models;
using UsersTeachers.Repositories;

namespace UsersTeachers.Repositories;

public class StudentRepository : BaseRepository<Student>, IStudentRepository
{

    public StudentRepository(UsersTeachersDbContext context) : base(context)
    {
    }
    public async Task<List<User>> GetAllUsersStudentAsync()
    {
        var usersWithStudentRole = await _context.Users.Where(u => u.UserRole == UserRole.Student).Include(u => u.Student).ToListAsync();

        return usersWithStudentRole;
    }

    public async Task<List<User>> GetAllUsersStudentAsync(int pageNumber, int pageSize)
    {
        int skip = pageNumber * pageSize;
        var usersWithStudentRole = await _context.Users.Where(u => u.UserRole == UserRole.Student).Include(u => u.Student).Skip(skip).Take(pageSize).ToListAsync();

        return usersWithStudentRole;
    }

    public async Task<Student?> GetByPhoneNumber(string? phoneNumber)
    {
        return await _context.Students.Where(p => p.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
    }

    public async Task<User?> GetStudentByUsernameAsync(string username)
    {
        return await _context.Users.Where(u => u.UserRole == UserRole.Student && u.Username == username).SingleOrDefaultAsync();
    }

    public async Task<List<Course>> GetStudentCourseAsync(int id)
    {
        List<Course> courses;
        courses = await _context.Students.Where(s => s.Id == id).SelectMany(s => s.Courses!).ToListAsync();

        return courses;
    }
}