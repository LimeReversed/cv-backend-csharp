using BackendCSharp;
using BackendCSharp.Database;
using BackendCSharp.Models;
using Microsoft.Data.Sqlite;

public class ImagePathRepository
{
    private static Func<SqliteDataReader, ImagePath> imagePathFactory = (reader) => new ImagePath
    (
        reader.GetFieldValue<long>(reader.GetOrdinal("project_id")),
        reader.GetFieldValue<string>(reader.GetOrdinal("image_path"))
    );

    private static DatabaseServiceTyped<ImagePath> databaseService = new DatabaseServiceTyped<ImagePath>(imagePathFactory);

    /// <param name="projectIds"></param>
    /// <returns>A Dictionary where the key represents a projectId and the value is a list of image paths that belong to that project</returns>
    public static Dictionary<long, List<ImagePath>> GetByProjectIds(List<long> projectIds)
    {
        string query = $"SELECT * FROM ProjectXImagePaths WHERE project_id IN ({projectIds.AsString()})";
        return databaseService.GetRelations(query, "project_id");
    }
}