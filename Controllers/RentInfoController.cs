using api.Interfaces;
using api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using api.Models;
using api.DTOs.RentDTO;
using System.Security.Claims;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentInfoController : ControllerBase
    {
        private readonly IRentInfo _rentRepository;

        public RentInfoController(IRentInfo rentRepository)
        {
            _rentRepository = rentRepository;
        }

        //all rents
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<RentInfo>>> GetAllRents()
        {
            var rents = await _rentRepository.GetAllRents();

            return Ok(rents);
        }

        //rents by user
        [HttpGet("{userId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<RentInfo>>> GetRentsByUserId(string userId)
        {
            var rents = await _rentRepository.GetRentsByUserId(userId);

            return Ok(rents);
        }

        //create for pet
        [HttpPost("create-for-pet/{petId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<RentInfo>> CreateRentForPet([FromBody] CreateRentDTO rentDTO, [FromRoute] int petId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
            {
                return Unauthorized("UserId and Email are required in the token.");
            }

            if (rentDTO == null)
            {
                return BadRequest("Invalid rent data.");
            }

            if (petId <= 0)
            {
                return BadRequest("Invalid PetId.");
            }

            var rent = await _rentRepository.CreateRentForPet(rentDTO, userId, email, petId);
            return CreatedAtAction(nameof(GetRentsByUserId), new { userId = rent.UserId }, rent);
        }

        //create for package
        [HttpPost("create-for-package/{packageId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<RentInfo>> CreateRentForPackage([FromBody] CreateRentDTO rentDTO, [FromRoute] int packageId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
            {
                return Unauthorized("UserId and Email are required in the token.");
            }

            if (rentDTO == null)
            {
                return BadRequest("Invalid rent data.");
            }

            if (packageId <= 0)
            {
                return BadRequest("Invalid PackageId.");
            }

            var rent = await _rentRepository.CreateRentForPackage(rentDTO, userId, email, packageId);
            return CreatedAtAction(nameof(GetRentsByUserId), new { userId = rent.UserId }, rent);
        }

    }
}