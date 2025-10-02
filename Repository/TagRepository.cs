using BackendCSharp.Models;
using BackendCSharp.Database;
using BackendCSharp;

public class TagRepository
{
    static public List<Tag> GetAll()
    {
        var resultRaw = Db.GetRows("SELECT * FROM Tags");
        var result = new List<Tag>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            result.Add(new Tag(row));
        }

        return result;

    }

    static public List<Tag> GetByIds(List<long> tagIds)
    {
        var resultRaw = Db.GetRows($"SELECT * FROM Tags WHERE id IN ({tagIds.AsString()})");
        var result = new List<Tag>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            result.Add(new Tag(row));
        }

        return result;

    }

    /// <param name="experienceIds"></param>
    /// <returns>A Dictionary where the key represents an experienceId and the value is a list of tags that belong to that experience</returns>
    static public Dictionary<long, List<Tag>> GetByExperienceIds(List<long> experienceIds)
    {
        string query = $"SELECT Tags.*, ExperienceXTags.experience_id FROM Tags JOIN ExperienceXTags ON Tags.id = ExperienceXTags.tag_id WHERE ExperienceXTags.experience_id IN ({experienceIds.AsString()})";
        var resultRaw = Db.GetRows(query);
        var result = new Dictionary<long, List<Tag>>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            long experienceId = (long)row["experience_id"];
            var tag = new Tag(row);

            if (result.ContainsKey(experienceId))
            {
                result[experienceId].Add(tag);
            }
            else
            {
                result.Add(experienceId, new List<Tag> { tag });
            }
        }

        return result;

    }
}