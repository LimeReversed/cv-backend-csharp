public class Project
{
    public Project(string name, string description, List<string> imagepaths, string link)
    {
        Name = name;
        Description = description;
        Imagepaths = imagepaths;
        Link = link;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Imagepaths { get; set; }
    public string Link {  get; set; }
}