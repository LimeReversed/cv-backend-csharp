using BackendCSharp.Models;

// Restrict to Model
public class DatabaseConverter<T>
{
    public List<T> ToList(List<Dictionary<string, object>> rows) 
    {
        var result = new List<T>();
        foreach (Dictionary<string, object> row in rows)
        {
            var instance = (T)Activator.CreateInstance(typeof(T), row);
            result.Add(instance);
        }

        return result;
    }
}
