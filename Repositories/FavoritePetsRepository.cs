using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Data;
using System.Security.AccessControl;
using api.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace api.Repositories
{
    public class FavoritePetsRepository : IFavoritePets
    {
        private readonly AppDbContext _context;

        public FavoritePetsRepository(AppDbContext context)
        {
            _context = context;
        }


        //userId
        public async Task<List<Pet>> GetFavoritePetsByUserId(string userId)
        {
            var favoritePets = await _context.FavoritePets
                .Where(fp => fp.UserId == userId && fp.Pet.IsDeleted == false)
                .Select(fp => fp.Pet)
                .ToListAsync();
            return favoritePets;
        }


        //is in fav
        public async Task<bool?> IsPetInFavorites(int petId, string userId)
        {
            var favorite = await _context.FavoritePets
                .FirstOrDefaultAsync(f => f.PetId == petId && f.UserId == userId);

            if (favorite == null)
            {
                return false;
            }
            return true;
        }

        //add
        public async Task<FavoritePets> AddToFavorite(int petId, string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User is not found");
            }

            var existingFavorite = await _context.FavoritePets.FirstOrDefaultAsync(fp => fp.UserId == userId && fp.PetId == petId);
            if (existingFavorite != null)
            {
                return existingFavorite;
            }

            var favoritePet = new FavoritePets
            {
                UserId = userId,
                PetId = petId
            };
            await _context.FavoritePets.AddAsync(favoritePet);
            await _context.SaveChangesAsync();
            return favoritePet;
        }

        //remove
        public async Task<bool> RemoveFromFavorite(int petId, string userId)
        {
            var favoritePet = await _context.FavoritePets
                .FirstOrDefaultAsync(f => f.PetId == petId && f.UserId == userId);
            if (favoritePet == null)
            {
                return false;
            }
            _context.FavoritePets.Remove(favoritePet);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}