public class Tag
{
    public Tag(int id, string name, int level, string category)
    {
        Id = id;
        Name = name;
        Level = level;
        Category = category;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public string Category {  get; set; }
}