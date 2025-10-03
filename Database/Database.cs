using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace BackendCSharp.Database
{
   

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

    public static class Db
    {
        //static public string connectionString = @"Data Source = E:\Programs\CV\backend-c#\Database\ResumeDatabase.db3";
        static public string connectionString = @"Data Source = C:\Emil\Backup\Programs\CV\cv-backend-csharp\Database\ResumeDatabase.db3";

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
                SqliteParameter parameter = new SqliteParameter(rowEntry[i].ParameterName, rowEntry[i].Value);
                command.Parameters.Add(parameter);
            }
        }

        /// <param name="query"></param>
        /// <returns>Return all rows found from the query</returns>
        static public List<Dictionary<string, object>> GetRows(string query, List<ColumnEntry> rowEntry = null)
        {
            using var connection = new SqliteConnection(connectionString);

            using SqliteCommand command = new SqliteCommand(query, connection);
            if (rowEntry != null) command.AddParameters(rowEntry);

            var rows = new List<Dictionary<string, object>>();

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                rows = ReadRows(reader);
            }
            catch (SqliteException e)
            {
                Debug.WriteLine(e.Message);
            }

            return rows;
        }

        /// <summary>
        /// Executes a query as a part of an existing connection, and optionally as a part of a transaction. 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Return all rows found from the query</returns>
        static public List<Dictionary<string, object>> GetRows(string query, SqliteConnection connection, List<ColumnEntry> rowEntry = null, SqliteTransaction transaction = null)
        {
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Transaction = transaction;
            if (rowEntry != null) command.AddParameters(rowEntry);

            using var reader = command.ExecuteReader();

            return ReadRows(reader);
        }

        /// <summary>
        /// A method made to show the relationship between two entities. 
        /// </summary>
        /// <param name="relationTableQuery">This method expects a query on a relationship table. One example is the ExperienceXProjects table 
        /// that contains a column for experienceId and another column for projectid.</param>
        /// <returns>
        /// Returns a Dictionary where the key represents the parent id and the value is a list of child ids.
        /// For example if an Experience contains several projects, then we have experienceId as a key, then a list of projectIds as value.
        /// </returns>
        static public Dictionary<long, List<long>> GetRelations(string relationTableQuery)
        {
            using var connection = new SqliteConnection(connectionString);
            using SqliteCommand command = new SqliteCommand(relationTableQuery, connection);

            var rows = new Dictionary<long, List<long>>();

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                rows = ReadRelations(reader);
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
        /// <returns>
        /// Returns a Dictionary where the key represents the parent id and the value is a list of child ids.
        /// For example if an Experience contains several projects, then we have experienceId as a key, then a list of projectIds as value.
        /// </returns>
        static public Dictionary<long, List<long>> GetRelations(string relationTableQuery, SqliteConnection connection, SqliteTransaction transaction = null)
        {
            using SqliteCommand command = new SqliteCommand(relationTableQuery, connection);
            command.Transaction = transaction;

            using var reader = command.ExecuteReader();

            return ReadRelations(reader);
        }

        static private List<Dictionary<string, object>> ReadRows(SqliteDataReader reader)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();

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

        static private Dictionary<long, List<long>> ReadRelations(SqliteDataReader reader)
        {
            var rows = new Dictionary<long, List<long>>();

            while (reader.Read())
            {
                
                long parentId = reader.GetInt64(0);
                long childId = reader.GetInt64(1);

                if (rows.ContainsKey(parentId))
                {
                    rows[parentId].Add(childId);
                }
                else
                {
                    rows.Add(parentId, new List<long> { childId });
                }
            }

            return rows;
        }
    }
}
