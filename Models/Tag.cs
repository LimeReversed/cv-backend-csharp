using Microsoft.Data.Sqlite;

public class Tag
{
    public Tag(long id, string name, long level, string category)
    {
        Id = id;
        Name = name;
        Level = level;
        Category = category;
    }

    public Tag(object id, object name, object level, object category)
    {
        Id = (long)id;
        Name = (string)name;
        Level = (long)level;
        Category = (string)category;
    }

    public Tag(SqliteDataReader reader)
    {
        Id = reader.GetFieldValue<long>(reader.GetOrdinal("id"));
        Name = reader.GetFieldValue<string>(reader.GetOrdinal("name"));
        Level = reader.GetFieldValue<long>(reader.GetOrdinal("level"));
        Category = reader.GetFieldValue<string>(reader.GetOrdinal("category"));
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public long Level { get; set; }
    public string Category {  get; set; }
}