using api.DTOs.SpecialPackageDTO;
using api.Models;
using AutoMapper;

namespace api.Mappers
{
    public class SpecialPackageMapper : Profile
    {
        public SpecialPackageMapper()
        {
            CreateMap<CreatePackageDTO, SpecialPackage>();
            CreateMap<SpecialPackage, CreatePackageDTO>();

            //CreateMap<UpdatePackageDTO, SpecialPackage>();
            //CreateMap<SpecialPackage, UpdatePackageDTO>();

        }
    }
}