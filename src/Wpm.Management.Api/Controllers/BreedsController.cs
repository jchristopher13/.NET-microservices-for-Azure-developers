using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BreedsController(ManagementDbContext dbContext, ILogger<BreedsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var all = await dbContext.Breeds.ToListAsync();

        return all!= null ? Ok(all) : NotFound();
    }

    [HttpGet("{id}", Name = nameof(GetBreedById))]
    public async Task<IActionResult> GetBreedById(int id)
    {
        var breed = await dbContext
            .Breeds
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();

        if (breed == null)
            return NotFound();

        return Ok(breed);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewBreed newBreed)
    {
        try{
            var breed = newBreed.ToBreed();
            var duplicate = await dbContext.Breeds
                .Where(b => b.Name.ToLower() == breed.Name.ToLower())
                .FirstOrDefaultAsync();

            if (duplicate != null)
            {
                return BadRequest($"{breed.Name} exists Id: {duplicate.Id}, Name: {duplicate.Name}");
            }


            await dbContext.Breeds.AddAsync(breed);
            await dbContext.SaveChangesAsync();

            return CreatedAtRoute(nameof(GetBreedById), new {id = breed.Id}, breed);
        }
        catch (Exception ex)
        {
            logger?.LogError(ex.ToString());
             return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}

public record NewBreed(string Name)
{
    public Breed ToBreed()
    {
        return new Breed(0, Name);
    }
}

