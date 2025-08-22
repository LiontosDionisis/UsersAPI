using AutoMapper;
using UsersTeachers.Data;
using UsersTeachers.DTO;

namespace UsersTeachers.Configuration;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<User, UserPatchDTO>().ReverseMap();
        CreateMap<User, UserLoginDTO>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
    }
}