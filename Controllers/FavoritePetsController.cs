
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using api.Repositories;
using System.Security.Claims;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritePetsController : ControllerBase
    {
        private readonly IFavoritePets _favoritePetsRepository;
        private readonly UserManager<User> _userManager;

        public FavoritePetsController(IFavoritePets favoritePetsService, UserManager<User> userManager)
        {
            _favoritePetsRepository = favoritePetsService;
            _userManager = userManager;
        }


        //get favorite pets
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetFavoritePets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pets = await _favoritePetsRepository.GetFavoritePetsByUserId(userId);
            return Ok(pets);
        }



        //status
        [HttpGet("status/{petId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetFavoriteStatus([FromRoute] int petId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isFavorite = await _favoritePetsRepository.IsPetInFavorites(petId, userId);

            if (isFavorite == null)
            {
                return NotFound("Pet not found.");
            }

            return Ok(isFavorite);
        }

        //add
        [HttpPost("add-to-favorites/{petId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToFavorites([FromRoute] int petId)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorite = await _favoritePetsRepository.AddToFavorite(petId, userId);

            if (favorite != null)
            {
                return Ok(favorite);
            }
            else
            {
                return BadRequest("Failed to add to favorites.");
            }
        }




        // remove
        [HttpDelete("remove-from-favorites/{petId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveFromFavorite([FromRoute] int petId)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var result = await _favoritePetsRepository.RemoveFromFavorite(petId, userId);

            if (!result)
            {
                return NotFound("Pet not found in favorites.");
            }
            return NoContent();
        }


    }


}
