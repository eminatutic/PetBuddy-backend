using api.Models;

namespace api.DTOs.SpecialPackageDTO
{
    public class PetInPackageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Age { get; set; }
        public Size Size { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public float AverageRating { get; set; }
        public AnimalType AnimalType { get; set; }
    }
}