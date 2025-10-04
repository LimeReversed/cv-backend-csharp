using Microsoft.Data.Sqlite;

namespace BackendCSharp.Models;

public class Experience
{
    public Experience(long id, string title, string? orgName, string? location, string from, string? to, string? tldr, string description, string type, List<Tag> tags, List<Project> projects)
    {
        Id = id;
        Title = title;
        OrgName = orgName;
        Location = location;
        From = from;
        To = to;
        Tldr = tldr;
        Description = description;
        Type = type;
        Tags = tags;
        Projects = projects;
    }

    public Experience(SqliteDataReader reader)
    {
        Id = reader.GetFieldValue<long>(reader.GetOrdinal("id"));
        Title = reader.GetFieldValue<string>(reader.GetOrdinal("title"));
        OrgName = reader.GetFieldValue<string>(reader.GetOrdinal("org_name"));
        Location = reader.GetFieldValue<string>(reader.GetOrdinal("location"));
        From = reader.GetFieldValue<string>(reader.GetOrdinal("from"));
        To = reader.GetFieldValue<string>(reader.GetOrdinal("to"));
        Tldr = reader.GetFieldValue<string>(reader.GetOrdinal("tldr"));
        Description = reader.GetFieldValue<string>(reader.GetOrdinal("description"));
        Type = reader.GetFieldValue<string>(reader.GetOrdinal("type"));
        Tags = new List<Tag>();
        Projects = new List<Project>();
    }

    public long Id { get; set; }
    public string Title { get; set; }
    public string? OrgName { get; set; }
    public string? Location { get; set; }
    public string From { get; set; }
    public string? To { get; set; }
    public string? Tldr { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Project> Projects { get; set; }
}