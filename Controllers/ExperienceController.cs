namespace BackendCSharp.Controllers;

using BackendCSharp.Models;
using BackendCSharp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

[ApiController]
[Route("[controller]")]
public class ExperienceController : ControllerBase
{
    private readonly ILogger<ExperienceController> _logger;

    public ExperienceController(ILogger<ExperienceController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    public IActionResult Get()
    {
        List<Experience> result = new List<Experience>();

        try
        {
            var experience = ExperienceRepository.GetAll();
            result = RepositoryHelpers.FillExperience(experience);
        }
        catch(SqliteException e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return StatusCode(StatusCodes.Status200OK, result);

    }

    [HttpGet("jobs")]
    public IActionResult GetJobs()
    {
        List<Experience> result = new List<Experience>();

        try
        {
            var experience = ExperienceRepository.GetJobs();
            result = RepositoryHelpers.FillExperience(experience);
        }
        catch (SqliteException e)
        {   Console.WriteLine(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return StatusCode(StatusCodes.Status200OK, result);
   
        
    }

    [HttpGet("education")]
    public IActionResult GetEducation()
    {
        List<Experience> result = new List<Experience>();

        try
        {
            var experience = ExperienceRepository.GetEducation();
            result = RepositoryHelpers.FillExperience(experience);
        }
        catch (SqliteException e)
        {   Console.WriteLine(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return StatusCode(StatusCodes.Status200OK, result);

    }

    [HttpGet("hobbies")]
    public IActionResult GetHobbies()
    {
        List<Experience> result = new List<Experience>();

        try
        {
            var experience = ExperienceRepository.GetHobbies();
            result = RepositoryHelpers.FillExperience(experience);
        }
        catch (SqliteException e)
        {   Console.WriteLine(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return StatusCode(StatusCodes.Status200OK, result);

    }
}
