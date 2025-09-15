using System.Drawing;
using System.Linq;
using api.Data;
using api.Models;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace api.GraphQL
{
    public class Query
    {
        private readonly AppDbContext _context;

        public Query(AppDbContext context)
        {
            _context = context;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<Pet> GetPets() =>
            _context.Pets.Where(p => !p.IsDeleted);


        [UseFiltering]
        [UseSorting]
        public IQueryable<Pet> GetPetsByType(AnimalType type) =>
            _context.Pets.Where(p => p.AnimalType == type && !p.IsDeleted);


        [UseFiltering]
        [UseSorting]
        public IQueryable<Pet> GetPetById(int petId)
        {
            return _context.Pets
                .Where(p => p.Id == petId && !p.IsDeleted);
        }


        //query {
        //  pets {
        //    id
        //    name
        //    age
        //    animalType
        //    price
        //    }
        //}


        //query {
        //  petsByType(type: DOG) {
        //    id
        //    name
        //    age
        //    animalType
        //    price
        //  }
        //}

        //query {
        //  petById(petId: 1) {
        //    id
        //    name
        //    age
        //    animalType
        //    price
        //    status
        //    size
        //    description
        //    imageUrl
        //  }
        //}



    }
}