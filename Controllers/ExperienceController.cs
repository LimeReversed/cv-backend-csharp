namespace BackendCSharp.Controllers;

using Microsoft.AspNetCore.Mvc;
using BackendCSharp.Models;
using BackendCSharp.Repositories;

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
    public List<Experience> Get()
    {
        var experience = ExperienceRepository.GetAll();
        return RepositoryHelpers.FillExperience(experience);

    }

    [HttpGet("jobs")]
    public List<Experience> GetJobs()
    {
        var experience = ExperienceRepository.GetJobs();
        return RepositoryHelpers.FillExperience(experience);
        
    }

    [HttpGet("education")]
    public List<Experience> GetEducation()
    {
        var experience = ExperienceRepository.GetEducation();
        return RepositoryHelpers.FillExperience(experience);

    }

    [HttpGet("hobbies")]
    public List<Experience> GetHobbies()
    {
        var experience = ExperienceRepository.GetHobbies();
        return RepositoryHelpers.FillExperience(experience);

    }
}
