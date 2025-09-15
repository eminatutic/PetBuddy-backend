using System.ComponentModel.DataAnnotations;

namespace api.DTOs.RentDTO
{
    public class CreateRentDTO
    {
        [Required]
        [Range(1, 365, ErrorMessage = "The number of days must be between 1 and 365.")]
        public int NumberOfDays { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The address cannot be longer than 255 characters.")]
        public string Address { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentMethod), ErrorMessage = "Invalid payment method.")]
        public PaymentMethod PaymentMethod { get; set; }


    }
}