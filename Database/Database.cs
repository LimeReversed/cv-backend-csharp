namespace BackendCSharp.Database;

using Microsoft.Data.Sqlite;
using System.Data;
using System.Diagnostics;

public class ColumnEntry
{
    public string Column { get; private set; }
    public object? Value { get; private set; }
    public string ParameterName { get; private set; }

    public ColumnEntry(string column, object? value, string? parameterName = null)
    {
        if (parameterName != null && !parameterName[0].Equals('@'))
        {
            throw new FormatException("parameterName must begin with @");
        }


        Column = AddBrackets(column.Trim());
        Value = value ?? DBNull.Value;
        ParameterName = parameterName ?? $"@{Column}";
    }

    private string AddBrackets(string name)
    {
        // Names such as column names or parameter names that include empty spaces and/or some other characters,
        // cause trouble if the name isn't bracketed. Therefore we make sure that it is bracketed. 
        if (!name[0].Equals('['))
        {
            name = $"[{name}";
        }

        if (!name[name.Length - 1].Equals(']'))
        {
            name = $"{name}]";
        }

        return name;
    }
}

public static class DatabaseExtensionMethods
{
    /// <summary>
    /// Adds parameters to the existing command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="rowEntry"></param>
    static public void AddParameters(this SqliteCommand command, List<ColumnEntry> rowEntry)
    {
        for (int i = 0; i < rowEntry.Count; i++)
        {
            // Remember that the parameter name here must be the same as the one given in the command text. 
            var parameter = new SqliteParameter(rowEntry[i].ParameterName, rowEntry[i].Value);
            command.Parameters.Add(parameter);
        }
    }
}

public abstract class DatabaseServiceAbstract<T>
{
    public static string connectionString = $"Data Source = {AppDomain.CurrentDomain.BaseDirectory}Database/ResumeDatabase.db3";

    /// <param name="query"></param>
    /// <returns>Return all rows found from the query</returns>
    public List<T> GetRows(string query, List<ColumnEntry>? rowEntry = null)
    {
        using var connection = new SqliteConnection(connectionString);

        using var command = new SqliteCommand(query, connection);
        if (rowEntry != null) command.AddParameters(rowEntry);

        var rows = new List<T>();

        connection.Open();
        using var reader = command.ExecuteReader();
        rows = ReadRows(reader);

        return rows;
    }

    /// <summary>
    /// Executes a query as a part of an existing connection, and optionally as a part of a transaction. 
    /// </summary>
    /// <param name="query"></param>
    /// <returns>Return all rows found from the query</returns>
    public List<T> GetRows(string query, SqliteConnection connection, List<ColumnEntry>? rowEntry = null, SqliteTransaction? transaction = null)
    {
        using var command = new SqliteCommand(query, connection);
        command.Transaction = transaction;
        if (rowEntry != null) command.AddParameters(rowEntry);

        using var reader = command.ExecuteReader();

        return ReadRows(reader);
    }

    protected abstract List<T> ReadRows(SqliteDataReader reader);
}

public class DatabaseServiceGeneric : DatabaseServiceAbstract<Dictionary<string, object>>
{
    protected override List<Dictionary<string, object>> ReadRows(SqliteDataReader reader)
    {
        var rows = new List<Dictionary<string, object>>();

        while (reader.Read())
        {
            var row = new Dictionary<string, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                object value = reader.GetValue(i);
                row.Add(columnName, value);
            }

            rows.Add(row);
        }

        return rows;
    }
}

public class DatabaseServiceTyped<T> : DatabaseServiceAbstract<T>
{
    private Func<SqliteDataReader, T> modelFactory;

    public DatabaseServiceTyped(Func<SqliteDataReader, T> modelFactory)
    {
        this.modelFactory = modelFactory;
    }


    protected override List<T> ReadRows(SqliteDataReader reader)
    {
        var rows = new List<T>();

        while (reader.Read())
        {
            T instance = modelFactory(reader);
            if (instance != null) { rows.Add(instance); }
        }

        return rows;
    }

    protected Dictionary<long, List<T>> ReadRelations(SqliteDataReader reader, string parentIdName)
    {
        var rows = new Dictionary<long, List<T>>();

        while (reader.Read())
        {

            long parentId = reader.GetFieldValue<long>(parentIdName);
            T instance = modelFactory(reader);

            if (rows.ContainsKey(parentId))
            {
                rows[parentId].Add(instance);
            }
            else
            {
                rows.Add(parentId, [instance]);
            }
        }

        return rows;
    }

    /// <summary>
    /// A method made to show the relationship between two entities. 
    /// </summary>
    /// <param name="relationTableQuery">This method expects a query on a relationship table. One example is the ExperienceXProjects table 
    /// that contains a column for experienceId and another column for projectid.</param>
    /// <param name="keyColumnName">The name of the database column, which values will be used as keys in the returned dictionary.</param>
    /// <returns>
    /// Returns a Dictionary where the key represents the parent id and the value is a list of child ids.
    /// For example if an Experience contains several projects, then we have experienceId as a key, then a list of projectIds as value.
    /// </returns>
    public Dictionary<long, List<T>> GetRelations(string relationTableQuery, string keyColumnName)
    {
        using var connection = new SqliteConnection(connectionString);
        using var command = new SqliteCommand(relationTableQuery, connection);

        var rows = new Dictionary<long, List<T>>();

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();

            rows = ReadRelations(reader, keyColumnName);
        }
        catch (SqliteException e)
        {
            Debug.WriteLine(e.Message);
        }

        return rows;
    }

    /// <summary>
    /// A method made to show the relationship between two entities. It executes a query as a part of an existing connection, and optionally as a part of a transaction. 
    /// </summary>
    /// <param name="relationTableQuery">This method expects a query on a relationship table. One example is the ExperienceXProjects table 
    /// that contains a column for experienceId and another column for projectid.</param>
    /// <param name="keyColumnName">The name of the database column, which values will be used as keys in the returned dictionary.</param>
    /// <returns>
    /// Returns a Dictionary where the key represents the parent id and the value is a list of child ids.
    /// For example if an Experience contains several projects, then we have experienceId as a key, then a list of projectIds as value.
    /// </returns>
    public Dictionary<long, List<T>> GetRelations(string relationTableQuery, string keyColumnName, SqliteConnection connection, SqliteTransaction? transaction = null)
    {
        using var command = new SqliteCommand(relationTableQuery, connection);
        command.Transaction = transaction;

        using var reader = command.ExecuteReader();

        return ReadRelations(reader, keyColumnName);
    }
}

public static class Db
{
    public static DatabaseServiceGeneric genericDatabaseService = new();
}

