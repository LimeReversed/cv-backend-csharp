public class Project
{
    public Project(int id, string name, string description, List<string> imagepaths, string link)
    {
        Id = id;
        Name = name;
        Description = description;
        Imagepaths = imagepaths;
        Link = link;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Imagepaths { get; set; }
    public string Link {  get; set; }
}