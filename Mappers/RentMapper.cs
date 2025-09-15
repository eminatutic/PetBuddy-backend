using AutoMapper;
using api.Models;
using api.DTOs.RentDTO;
namespace api.Mappers
{
    public class RentMapper : Profile
    {
        public RentMapper()
        {
            CreateMap<RentInfo, CreateRentDTO>();
            CreateMap<CreateRentDTO, RentInfo>();
        }
    }
}