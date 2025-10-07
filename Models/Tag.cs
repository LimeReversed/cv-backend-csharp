namespace BackendCSharp.Models;

public class Tag
{
    public Tag(long id, string name, long level, string category)
    {
        Id = id;
        Name = name;
        Level = level;
        Category = category;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public long Level { get; set; }
    public string Category {  get; set; }
}