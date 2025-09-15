using AutoMapper;
using api.Models;
using api.Dtos;
using api.Dtos.UserDTO;
using api.DTOs.UserDTO;

namespace api.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, ChangePasswordDTO>();
            CreateMap<ChangePasswordDTO, User>();

            CreateMap<User, LoginUserDTO>();
            CreateMap<LoginUserDTO, User>();

            CreateMap<RegisterUserDTO, User>();
            CreateMap<User, RegisterUserDTO>();


        }
    }
}