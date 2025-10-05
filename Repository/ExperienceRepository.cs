namespace BackendCSharp.Repositories;

using BackendCSharp;
using BackendCSharp.Database;
using BackendCSharp.Models;
using Microsoft.Data.Sqlite;

public class ExperienceRepository
{
    private static Func<SqliteDataReader, Experience> experienceFactory = (reader) => new Experience
    (
        reader.GetFieldValue<long>(reader.GetOrdinal("id")),
        reader.GetFieldValue<string>(reader.GetOrdinal("title")),
        reader.GetFieldValue<string>(reader.GetOrdinal("org_name")),
        reader.GetFieldValue<string>(reader.GetOrdinal("location")),
        reader.GetFieldValue<string>(reader.GetOrdinal("from")),
        reader.GetFieldValue<string>(reader.GetOrdinal("to")),
        reader.GetFieldValue<string>(reader.GetOrdinal("tldr")),
        reader.GetFieldValue<string>(reader.GetOrdinal("description")),
        reader.GetFieldValue<string>(reader.GetOrdinal("type")),
        new List<Tag>(),
        new List<Project>()
    );

    private static DatabaseServiceTyped<Experience> databaseService = new DatabaseServiceTyped<Experience>(experienceFactory);

    public static List<Experience> GetAll()
    {
        return databaseService.GetRows("SELECT * FROM Experience");
    }

    public static List<Experience> GetByIds(List<int> experienceIds)
    {
        return databaseService.GetRows($"SELECT * FROM Experience WHERE id IN ({experienceIds.AsString()})");
    }

    public static List<Experience> GetJobs()
    {
        return databaseService.GetRows("SELECT * FROM Experience WHERE type = 'Job'");
    }

    public static List<Experience> GetEducation()
    {
        return databaseService.GetRows("SELECT * FROM Experience WHERE type = 'Education'");
    }

    public static List<Experience> GetHobbies()
    {
        return databaseService.GetRows("SELECT * FROM Experience WHERE type = 'Hobby'");
    }
}