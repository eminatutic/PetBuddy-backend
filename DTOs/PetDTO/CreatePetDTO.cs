using System;
using System.ComponentModel.DataAnnotations;
using api.Models;

namespace api.DTOs.PetDTO
{
    public class CreatePetDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Age cannot be negative.")]
        public int Age { get; set; }

        [EnumDataType(typeof(Size), ErrorMessage = "Invalid.")]
        public Size Size { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, float.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public float Price { get; set; }

        [EnumDataType(typeof(AnimalType), ErrorMessage = "Invalid.")]
        public AnimalType AnimalType { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Name { get; set; }
    }
}