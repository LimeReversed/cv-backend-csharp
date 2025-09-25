using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Text;

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
        static public string connectionString = @"Data Source = E:\Programs\CV\backend-c#\Database\ResumeDatabase.db3";

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>The first row found from the query</returns>
        static public Dictionary<string, string> GetFirstRow(string query, List<ColumnEntry> rowEntry = null)
        {
            using var connection = new SqliteConnection(connectionString);

            using SqliteCommand command = new SqliteCommand(query, connection);
            if (rowEntry != null) command.AddParameters(rowEntry);

            Dictionary<string, string> columns = new Dictionary<string, string>();

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        if (!columns.ContainsKey(columnName))
                        {
                            string value = reader.GetString(i);
                            columns.Add(columnName, value);
                        }
                    }
                }
            }
            catch (SqliteException e)
            {
                Debug.WriteLine(e.Message);
            }

            return columns;
        }

        static public Dictionary<string, string> GetFirstRow(string query, SqliteConnection connection, List<ColumnEntry> rowEntry = null, SqliteTransaction transaction = null)
        {
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Transaction = transaction;

            if (rowEntry != null) command.AddParameters(rowEntry);

            Dictionary<string, string> columns = new Dictionary<string, string>();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    if (!columns.ContainsKey(columnName))
                    {
                        string value = reader.GetString(i);
                        columns.Add(columnName, value);
                    }
                }
            }           

            return columns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Return all rows found from the query</returns>
        static public List<Dictionary<string, string>> GetRows(string query, List<ColumnEntry> rowEntry = null)
        {
            using var connection = new SqliteConnection(connectionString);

            using SqliteCommand command = new SqliteCommand(query, connection);
            if (rowEntry != null) command.AddParameters(rowEntry);

            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Dictionary<string, string> row = new Dictionary<string, string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        string value = reader.GetString(i);
                        row.Add(columnName, value);
                    }

                    rows.Add(row);
                }
            }
            catch (SqliteException e)
            {
                Debug.WriteLine(e.Message);
            }

            return rows;
        }

        static public List<Dictionary<string, string>> GetRows(string query, SqliteConnection connection, List<ColumnEntry> rowEntry = null, SqliteTransaction transaction = null)
        {
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Transaction = transaction;
            if (rowEntry != null) command.AddParameters(rowEntry);

            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Dictionary<string, string> row = new Dictionary<string, string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    string value = reader.GetString(i);
                    row.Add(columnName, value);
                }

                rows.Add(row);
            }


            return rows;
        }
    }
}
