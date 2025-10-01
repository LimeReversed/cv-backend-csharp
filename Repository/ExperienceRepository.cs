using BackendCSharp.Models;
using BackendCSharp.Database;
using BackendCSharp;

public class ExperienceRepository
{
    static public List<Experience> GetAll()
    {
        var resultRaw = Db.GetRows("SELECT * FROM Experience");
        var result = new List<Experience>();
        foreach (Dictionary<string, string> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }

    static public List<Experience> GetByIds(List<int> ids)
    {
        var resultRaw = Db.GetRows($"SELECT * FROM Experience WHERE id IN ({ids.AsString()})");
        var result = new List<Experience>();
        foreach (Dictionary<string, string> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }

    static public List<Experience> GetJobs()
    {
        var resultRaw = Db.GetRows("SELECT * FROM Experience WHERE type = 'Job'");
        var result = new List<Experience>();
        foreach (Dictionary<string, string> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }

    static public List<Experience> GetEducation()
    {
        var resultRaw = Db.GetRows("SELECT * FROM Experience WHERE type = 'Education'");
        var result = new List<Experience>();
        foreach (Dictionary<string, string> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }

    static public List<Experience> GetHobbies()
    {
        var resultRaw = Db.GetRows("SELECT * FROM Experience WHERE type = 'Hobby'");
        var result = new List<Experience>();
        foreach (Dictionary<string, string> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }
}