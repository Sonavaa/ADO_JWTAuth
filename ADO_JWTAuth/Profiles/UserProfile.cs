using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.Models;
using AutoMapper;

namespace ADO_JWTAuth.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<User, GetUserDTO>();
            CreateMap<GetUserDTO, User>();

            CreateMap<User, UpdateUserDTO>();
            CreateMap<UpdateUserDTO, User>();
        }
    }
}
