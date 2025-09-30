using Microsoft.AspNetCore.Mvc;
using BackendCSharp.Models;
using BackendCSharp.Database;
using System.Collections.Generic;
using System.Diagnostics;

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
    public IEnumerable<Experience> Get()
    {
        string query = "SELECT * FROM Experience";
        return GetExperience(query);

    }

    [HttpGet("jobs")]
    public IEnumerable<Experience> GetJobs()
    {
        string query = "SELECT * FROM Experience WHERE type = 'Job'";
        return GetExperience(query);
        
    }

    [HttpGet("education")]
    public IEnumerable<Experience> GetEducation()
    {
        string query = "SELECT * FROM Experience WHERE type = 'Education'";
        return GetExperience(query);

    }

    [HttpGet("hobbies")]
    public IEnumerable<Experience> GetHobbies()
    {
        string query = "SELECT * FROM Experience WHERE type = 'Hobbies'";
        return GetExperience(query);

    }

    private IEnumerable<Experience> GetExperience(string query)
    {
        var experiences = ExperienceRepository.GetByQuery(query);
        List<int> experienceIds = (List<int>)experiences.Select(x => x.Id);

        // Maybe a join so I can get the project and experience_id in the same row? 
        string projectIdsQuery = $"SELECT * FROM ExperienceXProjects WHERE experience_id IN ({experienceIds.AsString()})";

        foreach (Experience experience in experiences)
        {
            //string projectIdsQuery = $"SELECT project_id FROM ExperienceXProjects WHERE experience_id={experience.Id}";
            var projectIDsRaw = Db.GetRows(projectIdsQuery);
            // Need a wherein statement, but I can't just put a dic<string, string> there I need them separated by comma. 

            // Kan ha contructFromDb som tar dictionary och g�r det till ett object. 
            // Beh�ver isf ett s�tt att bara ta ut ID. G�ra en separat get? Hur styr jag isf att det �r r�tt select statement?
            // Eller ha en metod som tar ut en kolumn fr�n dict och tar v�rdena p� det? Beh�ver isf convertera det. 

            // L�gga in v�rdena i kontruktorn direkt n�r vi tar detfr�n databasen? Kan det l�tt bli fel om jag l�gger till en kolumn?

            // Problemet nu �r v�l snarare att List inte kommer. 

            // Jag kan g�ra metod f�r att convertera till instans, men n�r det �r en lista? Var l�gger jag den koden?

            // Ha repository? GetByID? Och s� en repository f�r varje Model.D� kan jag f� ett f�rdig objekt direkt. Men �terigen, hur g�ra om lista av objekt?

            //Project[] projects = new Project[];

            List<int> projectIDs = new List<int>();

            foreach (Dictionary<string, string> projectID in projectIDsRaw)
            {
                string value = "";
                var values = projectID.TryGetValue("project_id", out value);
                projectIDs.Add(Convert.ToInt16(value));
            }

            string projectsQuery = $"SELECT * FROM Projects WHERE id IN ({projectIDs.AsString()})";
            var projectsRaw = Db.GetRows(projectsQuery);

            string tagsIdsQuery = $"SELECT tag_id FROM ExperienceXTags WHERE experience_id = {experience.Id}";
            var tagIdsRaw = Db.GetRows(tagsIdsQuery);

            List<int> tagIds = new List<int>();

            foreach (Dictionary<string, string> tagsID in tagIdsRaw)
            {
                string value = "";
                var values = tagsID.TryGetValue("tag_id", out value);
                tagIds.Add(Convert.ToInt16(value));
            }

            string tagsQuery = $"SELECT * FROM Tags WHERE id IN ({tagIds.AsString()})";
            var tagsRaw = Db.GetRows(tagsQuery);

            foreach (Dictionary<string, string> tag in tagsRaw)
            {
                experience.Tags.Add(new Tag(Convert.ToInt16(tag["id"]), tag["name"], Convert.ToInt16(tag["level"]), tag["category"]));
            }


            foreach (Dictionary<string, string> project in projectsRaw)
            {
                var newProject = new Project(Convert.ToInt16(project["id"]), project["name"], project["description"], new List<string>(), project["link"]);

                string imagePathQuery = $"SELECT image_path FROM ProjectsXImagePaths WHERE project_id = {project["id"]}";
                var imagePathsRaw = Db.GetRows(imagePathQuery);

                foreach (Dictionary<string, string> path in imagePathsRaw)
                {
                    newProject.Imagepaths.Add(path["image_path"]);
                }

                experience.Projects.Add(newProject);
            }

        }

        return experiences;
    }
}
