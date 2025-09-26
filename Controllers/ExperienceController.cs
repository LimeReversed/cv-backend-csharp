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

    [HttpGet(Name = "GetAllExperiences")]
    public IEnumerable<Experience> Get()
    {
        string query = "SELECT * FROM Experience";
        var rawResult = Db.GetRows(query);
        var result = new List<Experience>();
        foreach (var row in rawResult) {
            result.Add(new Experience(Convert.ToInt32(row["id"]), row["title"], row["org_name"], row["location"], row["from"], row["to"], row["tldr"], []));
        }

        foreach (Experience experience in result)
        {
            string projectIdsQuery = $"SELECT project_id FROM ExperienceXProjects WHERE experience_id={experience.Id}";
            var projectIDsRaw = Db.GetRows(projectIdsQuery);
            // Need a wherein statement, but I can't just put a dic<string, string> there I need them separated by comma. 
            
            // Kan ha contructFromDb som tar dictionary och gör det till ett object. 
            // Behöver isf ett sätt att bara ta ut ID. Göra en separat get? Hur styr jag isf att det är rätt select statement?
            // Eller ha en metod som tar ut en kolumn från dict och tar värdena på det? Behöver isf convertera det. 

            // Lägga in värdena i kontruktorn direkt när vi tar detfrån databasen? Kan det lätt bli fel om jag lägger till en kolumn?

            // Problemet nu är väl snarare att List inte kommer. 
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
            // Jag kan göra metod för att convertera till instans, men när det är en lista? Var lägger jag den koden?
        }

        return result;
    }
}
