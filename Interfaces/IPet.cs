using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;
using api.DTOs.PetDTO;
using api.Models;

namespace api.Interfaces
{
    public interface IPet
    {
        Task<List<Pet>> GetAllPetsAsync();
        Task<Pet?> GetPetByIdAsync(int petId);
        Task<Pet> CreatePetAsync(CreatePetDTO createPetDTO);
        Task<Pet?> UpdatePetAsync(int id, UpdatePetDTO updatePetDTO);
        Task<bool> DeletePetAsync(int id);
        Task<List<Pet>> GetPetByTypeAsync(AnimalType type);
        Task<List<Pet>> GetAvailablePetsForPackageAsync();
    }
}