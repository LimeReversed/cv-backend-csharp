namespace BackendCSharp.Models;

public class Experience
{
    public Experience(int id, string title, string? orgName, string? location, string from, string? to, string? tldr, List<Tag> tags, List<Project> projects, string type)
    {
        Id = id;
        Title = title;
        OrgName = orgName;
        Location = location;
        From = from;
        To = to;
        Tldr = tldr;
        this.Tags = tags;
        Projects = projects;
        Type = type;
    }

    public Experience(Dictionary<string, string> row)
    {
        Id = Convert.ToInt32(row["id"]);
        Title = row["title"];
        OrgName = row["org_name"];
        Location = row["location"];
        From = row["from"];
        To = row["to"];
        Tldr = row["tldr"];
        this.Tags = new List<Tag>();
        Projects = new List<Project>();
        Type = row["type"];
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string? OrgName { get; set; }
    public string? Location { get; set; }
    public string From { get; set; }
    public string? To { get; set; }
    public string? Tldr { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Project> Projects { get; set; }
    public string Type { get; set; }
}