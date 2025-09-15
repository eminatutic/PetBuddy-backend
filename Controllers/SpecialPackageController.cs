using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Models;
using api.DTOs.SpecialPackageDTO;
using api.Services;
using api.DTOs.PetDTO;
using Microsoft.AspNetCore.Authorization;
namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialPackageController : ControllerBase
    {
        private readonly ISpecialPackage _packageRepository;

        public SpecialPackageController(ISpecialPackage packageRepository)
        {
            _packageRepository = packageRepository;
        }


        //all
        [HttpGet("get-all-packages")]
        public async Task<IActionResult> GetAllPackages()
        {
            var packages = await _packageRepository.GetAllPackagesAsync();

            return Ok(packages);

        }


        //by id
        [HttpGet("get-package-by-id/{id}")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            var package = await _packageRepository.GetSpecialPackageByIdAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            return Ok(package);
        }

        //add
        [HttpPost("create-package")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePackage([FromBody] CreatePackageDTO createPackageDTO)
        {
            if (createPackageDTO == null || createPackageDTO.PetIds.Count == 0)
            {
                return BadRequest();
            }

            if (createPackageDTO.PetIds.Count < 2 || createPackageDTO.PetIds.Count > 3)
            {
                return NotFound("Package must have min 2 and max 3 pets");
            }

            var createdPackage = await _packageRepository.CreatePackageAsync(createPackageDTO, createPackageDTO.PetIds);
            return CreatedAtAction(nameof(GetPackageById), new { id = createdPackage.Id }, createdPackage);
        }


        //delete
        [HttpDelete("delete-package/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePackage(int id)
        {

            var deletePackage = await _packageRepository.DeletePackageAsync(id);

            if (!deletePackage)
            {
                return NotFound("Package not found");
            }

            return NoContent();
        }


        // update
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdatePackage(int id, [FromBody] UpdatePackageDTO updatePackageDTO)
        //{
        //    var package = await _packageRepository.UpdatePackageAsync(id, updatePackageDTO);

        //    if (package == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(package);
        //}

    }


}