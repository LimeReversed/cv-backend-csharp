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

    static public List<Project> GetByIds(List<long> ids)
    {
        var resultRaw = Db.GetRows($"SELECT * FROM Projects WHERE id IN ({ids.AsString()})");
        var result = new List<Project>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            result.Add(new Project(row));
        }

        return result;

    }

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