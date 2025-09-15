
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Data;
using System.Security.AccessControl;
using api.Interfaces;
using api.Mappers;
using AutoMapper;
using api.DTOs.PetDTO;

namespace api.Repositories
{
    public class PetRepository : IPet
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public PetRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //all pets
        public async Task<List<Pet>> GetAllPetsAsync()
        {
            return await _context.Pets
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }


        //pet by id
        public async Task<Pet?> GetPetByIdAsync(int petId)
        {
            return await _context.Pets.FindAsync(petId);
        }

        //add
        public async Task<Pet> CreatePetAsync(CreatePetDTO createPetDTO)
        {
            var pet = _mapper.Map<Pet>(createPetDTO);
            await _context.Pets.AddAsync(pet);
            await _context.SaveChangesAsync();
            return pet;
        }

        //update
        public async Task<Pet?> UpdatePetAsync(int id, UpdatePetDTO updatePetDTO)
        {
            var existingPet = await _context.Pets.FindAsync(id);
            if (existingPet != null)
            {
                if (updatePetDTO.Name != null) existingPet.Name = updatePetDTO.Name;
                if (updatePetDTO.Age != null) existingPet.Age = updatePetDTO.Age;
                if (updatePetDTO.Size != null) existingPet.Size = updatePetDTO.Size;
                if (updatePetDTO.Status != null) existingPet.Status = updatePetDTO.Status;
                if (updatePetDTO.Description != null) existingPet.Description = updatePetDTO.Description;
                if (updatePetDTO.Price != null) existingPet.Price = updatePetDTO.Price;

                if (updatePetDTO.ImageUrl != null) existingPet.ImageUrl = updatePetDTO.ImageUrl;

                await _context.SaveChangesAsync();
                return existingPet;
            }
            return null;
        }

        //delete
        public async Task<bool> DeletePetAsync(int id)
        {
            var deletePet = await _context.Pets.FindAsync(id);
            if (deletePet != null)
            {
                deletePet.IsDeleted = true;

                var packages = await _context.SpecialPackagePets
                  .Where(sp => sp.PetId == id)
                  .Select(sp => sp.SpecialPackage)
                  .Where(p => !p.IsDeleted)
                  .ToListAsync();


                foreach (var package in packages)
                {
                    package.IsDeleted = true;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        //pet by type
        public async Task<List<Pet>> GetPetByTypeAsync(AnimalType type)
        {
            return await _context.Pets
                .Where(pet => pet.AnimalType == type && !pet.IsDeleted)
                .ToListAsync();
        }
        public async Task<List<Pet>> GetAvailablePetsForPackageAsync()
        {

            var petsInActivePackages = _context.SpecialPackagePets
                .Where(sp => !sp.SpecialPackage.IsDeleted)
                .Select(sp => sp.PetId);

            var availablePets = await _context.Pets
                .Where(p => !petsInActivePackages.Contains(p.Id) && !p.IsDeleted && p.Status)
                .ToListAsync();

            return availablePets;
        }





    }
}
