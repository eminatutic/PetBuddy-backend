using api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace api.Interfaces
{
    public interface IFavoritePets
    {

        Task<List<Pet>> GetFavoritePetsByUserId(string userId);
        Task<FavoritePets> AddToFavorite(int petId, string userId);
        Task<bool> RemoveFromFavorite(int petId, string userId);
        Task<bool?> IsPetInFavorites(int petId, string userId);



    }
}