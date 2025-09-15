using api.DTOs.RentDTO;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Interfaces
{
    public interface IRentInfo
    {
        Task<List<RentInfo>> GetAllRents();
        Task<List<RentInfo>> GetRentsByUserId(string userId);
        Task<RentInfo> CreateRentForPet(CreateRentDTO rentDTO, string userId, string email, int petId);
        Task<RentInfo> CreateRentForPackage(CreateRentDTO rentDTO, string userId, string email, int packageId);
    }
}