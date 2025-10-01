namespace BackendCSharp.Models;

public class Experience
{
    public Experience(long id, string title, string? orgName, string? location, string from, string? to, string? tldr, List<Tag> tags, List<Project> projects, string type)
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

    public Experience(Dictionary<string, object> row)
    {
        Id = (long)row["id"];
        Title = (string)row["title"];
        OrgName = (string)row["org_name"];
        Location = (string)row["location"];
        From = (string)row["from"];
        To = (string)row["to"];
        Tldr = (string)row["tldr"];
        this.Tags = new List<Tag>();
        Projects = new List<Project>();
        Type = (string)row["type"];
    }

    public long Id { get; set; }
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