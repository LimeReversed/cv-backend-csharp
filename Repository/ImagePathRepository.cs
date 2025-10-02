using BackendCSharp.Database;
using BackendCSharp;

public class ImagePathRepository
{
    /// <param name="projectIds"></param>
    /// <returns>A Dictionary where the key represents a projectId and the value is a list of image paths that belong to that project</returns>
    static public Dictionary<long, List<string>> GetByProjectIds(List<long> projectIds)
    {
        List<Dictionary<string, object>> resultRaw = Db.GetRows($"SELECT * FROM ProjectXImagePaths WHERE project_id IN ({projectIds.AsString()})");
        var result = new Dictionary<long, List<string>>();
        foreach(Dictionary<string, object> row in resultRaw)
        {
            long projectId = (long)row["project_id"];
            string imagePath = (string)row["image_path"];

            if (result.ContainsKey(projectId))
            {
                result[projectId].Add(imagePath);
            }
            else
            {
                result.Add(projectId, new List<string> { imagePath });
            }
        }
        return result;
    }
}