using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class RentInfo
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public int? PetId { get; set; }
        public int? SpecialPackageId { get; set; }
        public int NumberOfDays { get; set; }
        public float TotalPrice { get; set; }
        public DateTime RentalDate { get; set; } = DateTime.Now;
        public string Address { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Email { get; set; }
        public virtual User User { get; set; }
        public virtual Pet Pet { get; set; }
        public virtual SpecialPackage SpecialPackage { get; set; }

    }
}
public enum PaymentMethod
{
    PayPal = 0,
    Cash = 1
}