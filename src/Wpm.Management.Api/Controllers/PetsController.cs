using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController(ManagementDbContext dbContext, ILogger<PetsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var all = await dbContext.Pets.Include(p => p.Breed).ToListAsync();

        return all != null ? Ok(all) : NotFound();
    }

    [HttpGet("{id}", Name = nameof(GetPetById))]
    public async Task<IActionResult> GetPetById(int id)
    {
        var pet = await dbContext.Pets.Include(p => p.Breed)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (pet == null)
            return NotFound($"No records matched with Id: {id}"); // Returns 404

        return Ok(pet);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewPet newPet)
    {
        if(newPet.Age < 0)
        {
            return BadRequest($"Pet's age should be a positive number {newPet.Age}");
        }

        var validBreed = await dbContext.Breeds.Where(b => b.Id == newPet.BreedId).AnyAsync();

        if (!validBreed)
        {
            return NotFound($"Invaild BreedId, {newPet.BreedId}");
        }

        try{
            var pet = newPet.ToPet();

            await dbContext.Pets.AddAsync(pet);
            await dbContext.SaveChangesAsync();

            return CreatedAtRoute(nameof(GetPetById), new {id = pet.Id }, newPet);
        }
        catch (Exception ex)
        {
            logger?.LogError(ex.ToString());
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}

public record NewPet(string Name, int Age, int BreedId)
{
    public Pet ToPet()
    {
        return new Pet() { 
            Age = Age,
            BreedId = BreedId,
            Name = Name
        };
    }
}
