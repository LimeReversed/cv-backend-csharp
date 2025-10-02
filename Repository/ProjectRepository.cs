using BackendCSharp.Models;
using BackendCSharp.Database;
using BackendCSharp;

public class ProjectRepository
{
    static public List<Project> GetAll()
    {
        var resultRaw = Db.GetRows("SELECT * FROM Projects");
        var result = new List<Project>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            result.Add(new Project(row));
        }

        return result;

    }

    static public List<Project> GetByIds(List<long> projectIds)
    {
        var resultRaw = Db.GetRows($"SELECT * FROM Projects WHERE id IN ({projectIds.AsString()})");
        var result = new List<Project>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            result.Add(new Project(row));
        }

        return result;

    }

    /// <param name="experienceIds"></param>
    /// <returns>A Dictionary where the key represents an experienceId and the value is a list of projects that belong to that experience</returns>
    static public Dictionary<long, List<Project>> GetByExperienceIds(List<long> experienceIds)
    {
        string query = $"SELECT Projects.*, ExperienceXProjects.experience_id FROM Projects JOIN ExperienceXProjects ON Projects.id = ExperienceXProjects.project_id WHERE ExperienceXProjects.experience_id IN ({experienceIds.AsString()})";
        var resultRaw = Db.GetRows(query);
        var result = new Dictionary<long, List<Project>>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            long experienceId = (long)row["experience_id"];
            var project = new Project(row);

            if (result.ContainsKey(experienceId))
            {
                result[experienceId].Add(project);
            }
            else
            {
                result.Add(experienceId, new List<Project> { project });
            }
        }

        return result;

    }
}