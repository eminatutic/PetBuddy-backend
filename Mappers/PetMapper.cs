using api.DTOs.PetDTO;
using AutoMapper;
using api.Models;

namespace api.Mappers
{
    public class PetMapper : Profile
    {
        public PetMapper()
        {
            CreateMap<CreatePetDTO, Pet>();
            CreateMap<Pet, CreatePetDTO>();

            CreateMap<UpdatePetDTO, Pet>();
            CreateMap<Pet, UpdatePetDTO>();


        }
    }
}