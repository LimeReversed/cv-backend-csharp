public class Project
{
    public Project(long id, string name, string description, List<string> imagepaths, string link)
    {
        Id = id;
        Name = name;
        Description = description;
        Imagepaths = imagepaths;
        Link = link;
    }

    public Project(Dictionary<string, object> row)
    {
        Id = (long)row["id"];
        Name = (string)row["name"];
        Description = (string)row["description"];
        Imagepaths = new List<string>();
        Link = (string)row["link"];
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Imagepaths { get; set; }
    public string Link {  get; set; }
}