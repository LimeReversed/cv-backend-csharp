using BackendCSharp.Models;
using BackendCSharp.Database;
using BackendCSharp;

public class ProjectRepository
{
    private static DatabaseServiceTyped<Project> databaseService = new DatabaseServiceTyped<Project>();

    static public List<Project> GetAll()
    {
        return databaseService.GetRows("SELECT * FROM Projects");
    }

    static public List<Project> GetByIds(List<long> projectIds)
    {
        return databaseService.GetRows($"SELECT * FROM Projects WHERE id IN ({projectIds.AsString()})");
    }

    /// <param name="experienceIds"></param>
    /// <returns>A Dictionary where the key represents an experienceId and the value is a list of projects that belong to that experience</returns>
    static public Dictionary<long, List<Project>> GetByExperienceIds(List<long> experienceIds)
    {
        string query = $"SELECT Projects.*, ExperienceXProjects.experience_id FROM Projects JOIN ExperienceXProjects ON Projects.id = ExperienceXProjects.project_id WHERE ExperienceXProjects.experience_id IN ({experienceIds.AsString()})";
        return databaseService.GetRelations(query, "experience_id");
    }
}