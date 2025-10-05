namespace BackendCSharp.Repositories;

using BackendCSharp;
using BackendCSharp.Database;
using BackendCSharp.Models;
using Microsoft.Data.Sqlite;

public class ProjectRepository
{
    private static Func<SqliteDataReader, Project> projectFactory = (reader) => new Project
(
    reader.GetFieldValue<long>(reader.GetOrdinal("id")),
    reader.GetFieldValue<string>(reader.GetOrdinal("name")),
    reader.GetFieldValue<string>(reader.GetOrdinal("description")),
    reader.GetFieldValue<string>(reader.GetOrdinal("link")),
    new List<ImagePath>()
);

    internal static DatabaseServiceTyped<Project> databaseService = new DatabaseServiceTyped<Project>(projectFactory);

    public static List<Project> GetAll()
    {
        return databaseService.GetRows("SELECT * FROM Projects");
    }

    public static List<Project> GetByIds(List<long> projectIds)
    {
        return databaseService.GetRows($"SELECT * FROM Projects WHERE id IN ({projectIds.AsString()})");
    }

    /// <param name="experienceIds"></param>
    /// <returns>A Dictionary where the key represents an experienceId and the value is a list of projects that belong to that experience</returns>
    public static Dictionary<long, List<Project>> GetByExperienceIds(List<long> experienceIds)
    {
        string query = $"SELECT Projects.*, ExperienceXProjects.experience_id FROM Projects JOIN ExperienceXProjects ON Projects.id = ExperienceXProjects.project_id WHERE ExperienceXProjects.experience_id IN ({experienceIds.AsString()})";
        return databaseService.GetRelations(query, "experience_id");
    }

    /// <param name="experienceIds"></param>
    /// <returns>A Dictionary where the key represents an experienceId and the value is a list of projects that belong to that experience</returns>
    internal static Dictionary<long, List<Project>> GetByExperienceIds(List<long> experienceIds, SqliteConnection connection, SqliteTransaction transaction = null)
    {
        string query = $"SELECT Projects.*, ExperienceXProjects.experience_id FROM Projects JOIN ExperienceXProjects ON Projects.id = ExperienceXProjects.project_id WHERE ExperienceXProjects.experience_id IN ({experienceIds.AsString()})";
        return databaseService.GetRelations(query, "experience_id", connection, transaction);
    }
}