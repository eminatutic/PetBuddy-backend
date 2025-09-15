using System;

namespace api.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public Size Size { get; set; }
        public bool Status { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string Description { get; set; }
        public float Price { get; set; }
        public AnimalType AnimalType { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public float AverageRating { get; set; } = 0f;
        public List<SpecialPackagePet> SpecialPackagePets { get; set; } = new List<SpecialPackagePet>();


    }

    public enum Size
    {
        Small = 0,
        Medium = 1,
        Large = 2
    }

    public enum AnimalType
    {
        Dog = 0,
        Cat = 1,
        Parrot = 2,
        Turtle = 3,
        Rabbit = 4,
        Hamster = 5
    }
}