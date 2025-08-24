using UsersTeachers.Data;
using UsersTeachers.DTO;
using UsersTeachers.Models;
using AutoMapper;
using System.Security.Claims;
using System.Text;
using UsersTeachers.Security;
using UsersTeachers.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace UsersTeachers.Services;

public class UserService : IUserService
{

    private readonly IUnitOfWork? _unitOfWork;
    private readonly ILogger<UserService>? _logger;
    private readonly IMapper? _mapper;

    public UserService(IUnitOfWork unitOfWork, ILogger<UserService>? logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public string CreateUserToken(int userId, string? userName, string? email, UserRole? userRole, string? appSecurityKey)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey!));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsInfo = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName!),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email!),
            new Claim(ClaimTypes.Role, userRole.ToString()!)
        };

        var jwtSecurityToken = new JwtSecurityToken(null, null, claimsInfo, DateTime.UtcNow, DateTime.UtcNow.AddHours(3), signingCredentials);

        var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return userToken;
    }

    public async Task DeleteUserAsync(int id)
    {
        bool deleted;

        try
        {
            deleted = await ((IBaseRepository<User>)_unitOfWork!.Users).DeleteAsync(id);
            if (!deleted)
            {
                throw new Exception("User was not found");
            }
        }
        catch (Exception e)
        {
            _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
            throw;
        }
    }

    public async Task<User?> GetUserByUsernameAsync(string usernmame)
    {
        User user;

        try
        {
            user = await _unitOfWork!.Users.GetByUsernameAsync(usernmame);
            _logger!.LogInformation("{Message}", "User: " + user + " found and returned.");
            return user;
        }
        catch (Exception e)
        {
            _logger.LogError("{Message}{Exception}", e.Message, e.StackTrace);
            throw;
        }
    }

    public async Task<User?> SignUpUserAsync(UserSignupDTO dto)
    {
        Student? student;
        Teacher? teacher;
        User? user;

        try
        {
            user = ExtractUser(dto);
            User? existingUser = await _unitOfWork!.Users.GetByUsernameAsync(user.Username!);

            if (existingUser != null) throw new Exception("User Already exists");

            user.Password = EncryptionUtil.Encrypt(user.Password!);
            //await _unitOfWork.Users.AddAsync(user);
            await ((IBaseRepository<User>)_unitOfWork.Users).AddAsync(user);


            if (user!.UserRole!.Value.ToString().Equals("Student"))
            {
                student = ExtractStudent(dto);
                if (await _unitOfWork.Students.GetByPhoneNumber(student.PhoneNumber) is not null)
                {
                    throw new Exception("Student exists.");
                }

                await ((IBaseRepository<Student>)_unitOfWork.Students).AddAsync(student);
                user.Student = student;
            }
            else if (user!.UserRole!.Value.ToString().Equals("Teacher"))
            {
                teacher = ExtractTeacher(dto);

                if (await _unitOfWork!.Teachers.GetByPhoneNumber(teacher.PhoneNumber) is not null) throw new Exception("Teacher already exists.");

                await ((IBaseRepository<Teacher>)_unitOfWork.Teachers).AddAsync(teacher);
            }
            else
            {
                throw new Exception("Invalid Role");
            }

            await _unitOfWork.SaveChangesAsync();
            _logger!.LogInformation("{Message}", "User: " + user + " signup success");
        }
        catch (Exception e)
        {
            _logger!.LogError("{Meesage}{Exception}", e.Message, e.StackTrace);
            throw;
        }

        return user;
    }

    public async Task<User?> UpdateUserAsync(int userId, UserDTO dto)
    {
        User? existingUser;
        User? user;

        try
        {
            existingUser = await ((IBaseRepository<User>)_unitOfWork!.Users).GetAsync(userId);
            if (existingUser == null) return null;

            var userToUpdate = _mapper!.Map<User>(dto);

            user = await _unitOfWork.Users.UpdateUserAsync(userId, userToUpdate);
            _logger!.LogInformation("{Message}", "User: " + user + " updated!!");

        }
        catch (Exception e)
        {
            _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
            throw;
        }

        return user;
    }

    public async Task<User?> UpdateUserPatchAsync(int userId, UserPatchDTO dto)
    {
        User? existingUser;
        try
        {
            existingUser = await ((IBaseRepository<User>)_unitOfWork!.Users).GetAsync(userId);
            if (existingUser == null) return null;

            if(dto.Username != null) existingUser.Username = dto.Username;
            if(dto.Email != null) existingUser.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Password)) existingUser.Password = EncryptionUtil.Encrypt(dto.Password!);

            await _unitOfWork.SaveChangesAsync();
            _logger!.LogInformation("{Message}", "User: " + existingUser + " updated!");

            return existingUser;
        }
        catch (Exception e)
        {
            _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
            throw;
        }
    }

    public async Task<User?> VerifyAndGetUserAsync(UserLoginDTO dto)
    {
        User? user = null;

        try
        {
            user = await _unitOfWork!.Users.GetUserAsync(dto.Username!, dto.Password!);
            _logger!.LogInformation("{Message}", "User: " + user + " found and returned.");
        }
        catch (Exception e)
        {
            _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
            throw;
        }
        return user;
    }

    private User ExtractUser(UserSignupDTO dto) {
        return new User()
        {
            Username = dto.Username,
            Password = dto.Password,
            Email = dto.Email,
            Firstname = dto.Firstname,
            Lastname = dto.Lastname,
            UserRole = dto.UserRole
        };
    }

    private Student ExtractStudent(UserSignupDTO? dto)
    {
        return new Student()
        {
            PhoneNumber = dto!.PhoneNumber,
            Institution = dto!.Institution
        };
    }

    private Teacher ExtractTeacher(UserSignupDTO? dto)
    {
        return new Teacher()
        {
            PhoneNumber = dto!.PhoneNumber,
            Institution = dto!.Institution
        };
    }
}