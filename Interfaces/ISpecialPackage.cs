using api.DTOs.SpecialPackageDTO;
using api.Models;

namespace api.Interfaces
{
    public interface ISpecialPackage
    {
        Task<List<SpecialPackage>> GetAllPackagesAsync();
        Task<SpecialPackage> CreatePackageAsync(CreatePackageDTO createPackageDTO, List<int> petIds);
        Task<SpecialPackage> GetSpecialPackageByIdAsync(int packageId);
        Task<bool> DeletePackageAsync(int packageId);
        //Task<SpecialPackage?> UpdatePackageAsync(int packageId, UpdatePackageDTO updatePackageDTO);
    }
}