namespace BackendCSharp.Repositories;

using BackendCSharp.Database;
using BackendCSharp.Models;
using Microsoft.Data.Sqlite;

public class TagRepository
{
    private static Func<SqliteDataReader, Tag> tagFactory = (reader) => new Tag
    (
        reader.GetFieldValue<long>(reader.GetOrdinal("id")),
        reader.GetFieldValue<string>(reader.GetOrdinal("name")),
        reader.GetFieldValue<long>(reader.GetOrdinal("level")),
        reader.GetFieldValue<string>(reader.GetOrdinal("category"))
    );

    internal static DatabaseServiceTyped<Tag> databaseService = new DatabaseServiceTyped<Tag>(tagFactory);

    public static List<Tag> GetAll()
    {
        return databaseService.GetRows("SELECT * FROM Tags");
    }

    public static List<Tag> GetByIds(List<long> tagIds)
    {
        return databaseService.GetRows($"SELECT * FROM Tags WHERE id IN ({tagIds.AsString()})");
    }

    /// <param name="experienceIds"></param>
    /// <returns>A Dictionary where the key represents an experienceId and the value is a list of tags that belong to that experience</returns>
    public static Dictionary<long, List<Tag>> GetByExperienceIds(List<long> experienceIds)
    {
        string query = $"SELECT Tags.*, ExperienceXTags.experience_id FROM Tags JOIN ExperienceXTags ON Tags.id = ExperienceXTags.tag_id WHERE ExperienceXTags.experience_id IN ({experienceIds.AsString()})";
        return databaseService.GetRelations(query, "experience_id");
    }

    /// <param name="experienceIds"></param>
    /// <returns>A Dictionary where the key represents an experienceId and the value is a list of tags that belong to that experience</returns>
    internal static Dictionary<long, List<Tag>> GetByExperienceIds(List<long> experienceIds, SqliteConnection connection, SqliteTransaction? transaction = null)
    {
        string query = $"SELECT Tags.*, ExperienceXTags.experience_id FROM Tags JOIN ExperienceXTags ON Tags.id = ExperienceXTags.tag_id WHERE ExperienceXTags.experience_id IN ({experienceIds.AsString()})";
        return databaseService.GetRelations(query, "experience_id", connection, transaction);
    }
}