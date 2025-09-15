using api.Data;
using api.DTOs.SpecialPackageDTO;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{

    public class SpecialPackageRepository : ISpecialPackage
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SpecialPackageRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //all
        public async Task<List<SpecialPackage>> GetAllPackagesAsync()
        {
            return await _context.SpecialPackages
                .Where(sp => !sp.IsDeleted)
                .Include(sp => sp.SpecialPackagePets)
                    .ThenInclude(spp => spp.Pet)
                .Select(sp => new SpecialPackage
                {
                    Id = sp.Id,
                    Description = sp.Description,
                    Price = sp.Price,
                    PackageType = sp.PackageType,
                    IsDeleted = sp.IsDeleted,
                    SpecialPackagePets = sp.SpecialPackagePets
                })
                .ToListAsync();
        }



        //add
        public async Task<SpecialPackage> CreatePackageAsync(CreatePackageDTO createPackageDTO, List<int> petIds)
        {
            if (petIds == null || petIds.Count < 2 || petIds.Count > 3)
                throw new ArgumentException("Package must have min 2 and max 3 pets");

            var pets = await _context.Pets.Where(p => petIds.Contains(p.Id)).ToListAsync();


            if (pets.Count != petIds.Count)
                throw new ArgumentException("Invalid pet");

            var package = _mapper.Map<SpecialPackage>(createPackageDTO);

            package.SpecialPackagePets = pets.Select(pet => new SpecialPackagePet
            {
                PetId = pet.Id,
                SpecialPackage = package
            }).ToList();

            await _context.SpecialPackages.AddAsync(package);
            await _context.SaveChangesAsync();

            return package;
        }

        //by id
        public async Task<SpecialPackage?> GetSpecialPackageByIdAsync(int packageId)
        {
            return await _context.SpecialPackages
                .Include(sp => sp.SpecialPackagePets)
                    .ThenInclude(spp => spp.Pet)
                .FirstOrDefaultAsync(sp => sp.Id == packageId);
        }


        //delete
        public async Task<bool> DeletePackageAsync(int packageId)
        {
            var package = await _context.SpecialPackages
                .FirstOrDefaultAsync(sp => sp.Id == packageId);

            if (package == null)
                return false;

            package.IsDeleted = true;

            await _context.SaveChangesAsync();

            return true;
        }




        //update
        //public async Task<SpecialPackage?> UpdatePackageAsync(int id, UpdatePackageDTO updatePackageDTO)
        //{
        //    if (updatePackageDTO.Price <= 0)
        //        throw new ArgumentException("Price must be greater than 0.");


        //    if (string.IsNullOrWhiteSpace(updatePackageDTO.Description))
        //        throw new ArgumentException("description can not be empty");


        //    var package = await _context.SpecialPackages.FirstOrDefaultAsync(sp => sp.Id == id);
        //    if (package == null)
        //        return null;

        //    package.Price = updatePackageDTO.Price;
        //    package.Description = updatePackageDTO.Description;



        //    await _context.SaveChangesAsync();

        //    return package;
        //}


    }
}