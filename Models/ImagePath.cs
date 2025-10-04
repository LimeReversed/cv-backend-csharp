using Microsoft.Data.Sqlite;
using System.Xml.Linq;

public class  ImagePath
{
    public ImagePath(long projectId, string imagePath)
    {
        ProjectId = projectId;
        Path = imagePath;
    }

    public ImagePath(SqliteDataReader reader)
    {
        ProjectId = reader.GetFieldValue<long>(reader.GetOrdinal("project_id"));
        Path = reader.GetFieldValue<string>(reader.GetOrdinal("image_path"));
    }

    public long ProjectId { get; set; }
    public string Path {  get; set; }
}