public class  ImagePath
{
    public ImagePath(long projectId, string imagePath)
    {
        ProjectId = projectId;
        Path = imagePath;
    }

    public ImagePath(Dictionary<string, object> row)
    {
        ProjectId = (long)row["project_id"];
        Path = (string)row["image_path"];
    }

    public long ProjectId { get; set; }
    public string Path {  get; set; }
}