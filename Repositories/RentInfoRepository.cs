using Microsoft.EntityFrameworkCore;

using api.Data;
using api.Interfaces;
using AutoMapper;
using api.Models;
using api.DTOs.RentDTO;

namespace api.Repositories
{
    public class RentInfoRepository : IRentInfo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RentInfoRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //all rents
        public async Task<List<RentInfo>> GetAllRents()
        {
            return await _context.RentsInfo
                .Include(r => r.User)
                .Include(r => r.Pet)
                .Include(r => r.SpecialPackage)
                    .ThenInclude(sp => sp.SpecialPackagePets)
                        .ThenInclude(spp => spp.Pet)
                .ToListAsync();
        }


        //rents for user
        public async Task<List<RentInfo>> GetRentsByUserId(string userId)
        {
            return await _context.RentsInfo
                .Where(r => r.UserId == userId)
                .Include(r => r.Pet)
                .Include(r => r.SpecialPackage)
                    .ThenInclude(sp => sp.SpecialPackagePets)
                        .ThenInclude(spp => spp.Pet)
                .ToListAsync();
        }

        //create rent for pet
        public async Task<RentInfo> CreateRentForPet(CreateRentDTO rentDTO, string userId, string email, int petId)
        {
            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId);

            if (pet == null)
            {
                return null;
            }

            var rent = _mapper.Map<RentInfo>(rentDTO);
            rent.UserId = userId;
            rent.Email = email;
            rent.PetId = petId;
            rent.TotalPrice = rentDTO.NumberOfDays * pet.Price;

            pet.Status = false;
            var packages = await _context.SpecialPackagePets
                .Where(sp => sp.PetId == petId)
                .Select(sp => sp.SpecialPackage)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            foreach (var package in packages)
            {
                package.IsDeleted = true;
            }


            _context.RentsInfo.Add(rent);

            await _context.SaveChangesAsync();

            return rent;
        }


        //create rent for package
        public async Task<RentInfo> CreateRentForPackage(CreateRentDTO rentDTO, string userId, string email, int packageId)
        {
            var specialPackage = await _context.SpecialPackages
                .Include(sp => sp.SpecialPackagePets)
                    .ThenInclude(spp => spp.Pet)
                .FirstOrDefaultAsync(sp => sp.Id == packageId);

            if (specialPackage == null)
            {
                return null;
            }

            specialPackage.IsDeleted = true;

            
            foreach (var spp in specialPackage.SpecialPackagePets)
            {
                if (spp.Pet != null)
                {
                    spp.Pet.Status = false;
                }
            }

            var rent = _mapper.Map<RentInfo>(rentDTO);
            rent.UserId = userId;
            rent.Email = email;
            rent.SpecialPackageId = packageId;
            rent.TotalPrice = rentDTO.NumberOfDays * specialPackage.Price;

            _context.RentsInfo.Add(rent);

            await _context.SaveChangesAsync();

            return rent;
        }


    }
}