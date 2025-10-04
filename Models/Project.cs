using Microsoft.Data.Sqlite;

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

    public Project(SqliteDataReader reader)
    {
        Id = reader.GetFieldValue<long>(reader.GetOrdinal("id"));
        Name = reader.GetFieldValue<string>(reader.GetOrdinal("name"));
        Description = reader.GetFieldValue<string>(reader.GetOrdinal("description"));
        Link = reader.GetFieldValue<string>(reader.GetOrdinal("link"));
        Imagepaths = new List<ImagePath>();
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Link {  get; set; }
    public List<ImagePath> Imagepaths { get; set; }
}