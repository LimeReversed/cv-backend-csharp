using BackendCSharp;
using BackendCSharp.Database;
using BackendCSharp.Models;
using Microsoft.Data.Sqlite;
using System.Diagnostics;

public class ExperienceRepository
{
    public static List<Experience> GetAll()
    {
        var resultRaw = Db.GetRows("SELECT * FROM Experience");
        var result = new List<Experience>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }

    public static List<Experience> GetByIds(List<int> experienceIds)
    {
        var resultRaw = Db.GetRows($"SELECT * FROM Experience WHERE id IN ({experienceIds.AsString()})");
        var result = new List<Experience>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }

    public static List<Experience> GetJobs()
    {
        var resultRaw = Db.GetRows("SELECT * FROM Experience WHERE type = 'Job'");
        var result = new List<Experience>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }

    public static List<Experience> GetEducation()
    {
        var resultRaw = Db.GetRows("SELECT * FROM Experience WHERE type = 'Education'");
        var result = new List<Experience>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }

    public static List<Experience> GetHobbies()
    {
        var resultRaw = Db.GetRows("SELECT * FROM Experience WHERE type = 'Hobby'");
        var result = new List<Experience>();
        foreach (Dictionary<string, object> row in resultRaw)
        {
            result.Add(new Experience(row));
        }

        return result;

    }
}