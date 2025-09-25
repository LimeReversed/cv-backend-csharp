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

    [HttpGet(Name = "GetAllExperiences")]
    public IEnumerable<Experience> Get()
    {
        string query = "SELECT * FROM Experience";
        var rawResult = Db.GetRows(query);
        var result = new List<Experience>();
        foreach (var row in rawResult) {
            result.Add(new Experience(Convert.ToInt32(row["id"]), row["title"], row["org_name"], row["location"], row["from"], row["to"], row["tldr"], []));
        }

        return result;
    }
}
