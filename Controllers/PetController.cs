using api.DTOs.PetDTO;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPet _petRepository;
        public PetController(IPet petService)
        {
            _petRepository = petService;
        }



        //all pets

        [HttpGet("all-pets")]
        public async Task<IActionResult> GetPets()
        {
            var pets = await _petRepository.GetAllPetsAsync();
            return Ok(pets);
        }


        //pet by id

        [HttpGet("{petId}")]
        public async Task<IActionResult> GetPet([FromRoute] int petId)
        {
            var pet = await _petRepository.GetPetByIdAsync(petId);
            return Ok(pet);

        }


        //add
        [HttpPost("add-pet")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePet([FromBody] CreatePetDTO createPetDTO)
        {
            if (createPetDTO == null)
                return BadRequest("Error");
            var newPet = await _petRepository.CreatePetAsync(createPetDTO);
            return CreatedAtAction(nameof(CreatePet), new { id = newPet.Id }, newPet);
        }


        //update
        [HttpPut("update-pet/{petId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePet(int petId, UpdatePetDTO updatePetDTO)
        {
            if (updatePetDTO == null)
                return BadRequest("Error");

            var updatedPet = await _petRepository.UpdatePetAsync(petId, updatePetDTO);
            if (updatedPet != null)
                return Ok(updatedPet);
            return NotFound();
        }


        //delete
        [HttpDelete("delete-pet/{petId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePet([FromRoute] int petId)
        {
            var deletePet = await _petRepository.DeletePetAsync(petId);
            if (deletePet)
                return NoContent();
            return NotFound();
        }




        //pets by type
        [HttpGet("pets-by-type/{type}")]
        public async Task<IActionResult> GetPetsByType([FromRoute] AnimalType type)
        {
            var pets = await _petRepository.GetPetByTypeAsync(type);

            return Ok(pets);
        }

        //for package
        [HttpGet("available-for-package")]
        public async Task<IActionResult> GetAvailablePetsForPackage()
        {
            var pets = await _petRepository.GetAvailablePetsForPackageAsync();
            return Ok(pets);
        }
    }
}