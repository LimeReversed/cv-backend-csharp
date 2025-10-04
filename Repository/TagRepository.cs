using BackendCSharp.Models;
using BackendCSharp.Database;
using BackendCSharp;

public class TagRepository
{
    private static DatabaseServiceTyped<Tag> databaseService = new DatabaseServiceTyped<Tag>();

    static public List<Tag> GetAll()
    {
        return databaseService.GetRows("SELECT * FROM Tags");
    }

    static public List<Tag> GetByIds(List<long> tagIds)
    {
        return databaseService.GetRows($"SELECT * FROM Tags WHERE id IN ({tagIds.AsString()})");
    }

    /// <param name="experienceIds"></param>
    /// <returns>A Dictionary where the key represents an experienceId and the value is a list of tags that belong to that experience</returns>
    static public Dictionary<long, List<Tag>> GetByExperienceIds(List<long> experienceIds)
    {
        string query = $"SELECT Tags.*, ExperienceXTags.experience_id FROM Tags JOIN ExperienceXTags ON Tags.id = ExperienceXTags.tag_id WHERE ExperienceXTags.experience_id IN ({experienceIds.AsString()})";
        return databaseService.GetRelations(query, "experience_id");
    }
}