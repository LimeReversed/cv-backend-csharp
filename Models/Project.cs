namespace BackendCSharp.Models;

public class Project
{
    public Project(long id, string name, string description, string link, List<ImagePath> imagepaths)
    {
        Id = id;
        Name = name;
        Description = description;
        Link = link;
        Imagepaths = imagepaths;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Link {  get; set; }
    public List<ImagePath> Imagepaths { get; set; }
}