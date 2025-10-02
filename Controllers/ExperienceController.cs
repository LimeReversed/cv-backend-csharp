using Microsoft.AspNetCore.Mvc;
using BackendCSharp.Models;
using BackendCSharp.Database;

namespace BackendCSharp.Controllers;

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
        return PackageExperience(experience);

    }

    [HttpGet("jobs")]
    public List<Experience> GetJobs()
    {
        var experience = ExperienceRepository.GetJobs();
        return PackageExperience(experience);
        
    }

    [HttpGet("education")]
    public List<Experience> GetEducation()
    {
        var experience = ExperienceRepository.GetEducation();
        return PackageExperience(experience);

    }

    [HttpGet("hobbies")]
    public List<Experience> GetHobbies()
    {
        var experience = ExperienceRepository.GetHobbies();
        return PackageExperience(experience);

    }

    private List<Experience> PackageExperience(List<Experience> experience)
    {
        /*
         * I debated whether to add a transaction for all of these calls to the database. 
         * The reason being that experience, projects and image paths are all connected, and what if one changes after getting one but before getting the other?
         * There is a risk for dirty reads. I could start a transaction here, but a controller or a repository call should not care about the implementation of the storage. 
         * Therefore I can only have code that is connected to the sqlite database inside of repositories. I could call everything I need inside of the ExperienceRepository, but then
         * I would repeat a lot of code that is in the other repositories. So Repository pattern seems to cause issues with transactions together with DRY (Don't repeat yourself).
         */

        var experiences = ExperienceRepository.GetAll();
        var experienceIds = experiences.Select(x => x.Id).ToList();
        
        var projectsByExperienceIds = ProjectRepository.GetByExperienceIds(experienceIds);
        var tagsByExperienceIds = TagRepository.GetByExperienceIds(experienceIds);

        var projectIds = projectsByExperienceIds.SelectMany(keyValuePair => keyValuePair.Value).Select(project => project.Id).ToList();
        var imagePathsByProjectIds = ImagePathRepository.GetByProjectIds(projectIds);

        foreach (Experience ex in experiences)
        {
            // Some experiences don't have projects, so some experienceIDs will not be in represented in projectsByExperienceIds.
            // So we first need to check if the projectsByExperienceIds dictionary contains that experienceId. 
            if (projectsByExperienceIds.ContainsKey(ex.Id)) { ex.Projects = projectsByExperienceIds[ex.Id]; }
            if (tagsByExperienceIds.ContainsKey(ex.Id)) { ex.Tags = tagsByExperienceIds[ex.Id]; }

            foreach (Project project in ex.Projects)
            {
                if (imagePathsByProjectIds.ContainsKey(project.Id)) { project.Imagepaths = imagePathsByProjectIds[project.Id]; }
            }
        }

        return experiences;
    }
}
