using BackendCSharp.Models;
using BackendCSharp.Database;

public class ExperienceRepository
{
    static public List<Experience> GetByQuery(string query)
    {
        var resultRaw = Db.GetRows(query);
        var result = new List<Experience>();
        foreach (Dictionary<string, string> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }
}