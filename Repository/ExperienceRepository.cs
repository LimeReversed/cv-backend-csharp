using BackendCSharp;
using BackendCSharp.Database;
using BackendCSharp.Models;

// https://stackoverflow.com/questions/6529611/c-sharp-create-new-t

public class ExperienceRepository
{
    private static DatabaseConverter<Experience > rowsConverter = new DatabaseConverter<Experience>();

    public static List<Experience> GetAll()
    {
        var rows = Db.GetRows("SELECT * FROM Experience");
        return rowsConverter.ToList(rows);
    }

    public static List<Experience> GetByIds(List<int> experienceIds)
    {
        var rows = Db.GetRows($"SELECT * FROM Experience WHERE id IN ({experienceIds.AsString()})");
        return rowsConverter.ToList(rows);
    }

    public static List<Experience> GetJobs()
    {
        var rows = Db.GetRows("SELECT * FROM Experience WHERE type = 'Job'");
        return rowsConverter.ToList(rows);
    }

    public static List<Experience> GetEducation()
    {
        var rows = Db.GetRows("SELECT * FROM Experience WHERE type = 'Education'");
        return rowsConverter.ToList(rows);
    }

    public static List<Experience> GetHobbies()
    {
        var rows = Db.GetRows("SELECT * FROM Experience WHERE type = 'Hobby'");
        return rowsConverter.ToList(rows);
    }
}