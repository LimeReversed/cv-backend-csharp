namespace BackendCSharp.Models;

public class Experience
{
    public Experience(int id, string title, string? orgName, string? location, string from, string? to, string? tldr, int[] tags)
    {
        Id = id;
        Title = title;
        OrgName = orgName;
        Location = location;
        From = from;
        To = to;
        Tldr = tldr;
        this.tags = tags;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string? OrgName { get; set; }
    public string? Location { get; set; }
    public string From { get; set; }
    public string? To { get; set; }
    public string? Tldr { get; set; }
    public int[] tags { get; set; }
}