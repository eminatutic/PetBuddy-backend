using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
namespace api.Models
{
    public class FavoritePets
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }

    }
}