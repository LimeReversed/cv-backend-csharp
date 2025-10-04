using BackendCSharp;
using BackendCSharp.Database;
using BackendCSharp.Models;

// https://stackoverflow.com/questions/6529611/c-sharp-create-new-t

public class ExperienceRepository
{
    private static DatabaseServiceTyped<Experience> databaseService = new DatabaseServiceTyped<Experience>();

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