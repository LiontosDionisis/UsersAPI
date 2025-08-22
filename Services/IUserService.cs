using UsersTeachers.Data;
using UsersTeachers.DTO;
using UsersTeachers.Models;

namespace UsersTeachers.Services;

public interface IUserService
{
    Task<User?> SignUpUserAsync(UserSignupDTO dto);
    Task<User?> VerifyAndGetUserAsync(UserLoginDTO dto);
    Task<User?> UpdateUserAsync(int userId, UserDTO dto);
    Task<User?> UpdateUserPatchAsync(int userId, UserPatchDTO dto);
    Task<User?> GetUserByUsernameAsync(string usernmame);
    Task DeleteUserAsync(int id);
    string CreateUserToken(int userId, string? userName, string? email, UserRole? userRole, string? appSecurityKey);
}