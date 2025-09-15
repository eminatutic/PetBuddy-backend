using System.ComponentModel.DataAnnotations;

namespace api.DTOs.SpecialPackageDTO
{
    public class CreatePackageDTO
    {
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, float.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }
        public string PackageType { get; set; }

        [Required(ErrorMessage = "You must add 2 or 3 pets to the package.")]
        [MinLength(2, ErrorMessage = "The package must contain at least 2 pets.")]
        [MaxLength(3, ErrorMessage = "The package cannot contain more than 3 pets.")]
        public List<int> PetIds { get; set; } = new List<int>();
    }
}