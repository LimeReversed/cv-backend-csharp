using BackendCSharp;
using BackendCSharp.Database;
using BackendCSharp.Models;

public class ImagePathRepository
{
    private static DatabaseServiceTyped<ImagePath> databaseService = new DatabaseServiceTyped<ImagePath>();

    /// <param name="projectIds"></param>
    /// <returns>A Dictionary where the key represents a projectId and the value is a list of image paths that belong to that project</returns>
    static public Dictionary<long, List<ImagePath>> GetByProjectIds(List<long> projectIds)
    {
        string query = $"SELECT * FROM ProjectXImagePaths WHERE project_id IN ({projectIds.AsString()})";
        return databaseService.GetRelations(query, "project_id");
    }
}