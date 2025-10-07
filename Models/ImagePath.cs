namespace BackendCSharp.Models;

public class  ImagePath
{
    public ImagePath(long projectId, string imagePath)
    {
        ProjectId = projectId;
        Path = imagePath;
    }

    public long ProjectId { get; set; }
    public string Path {  get; set; }
}