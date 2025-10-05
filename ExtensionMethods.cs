namespace BackendCSharp;

using System.Text;

public static class Extensions
{
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="separator"></param>
    /// <param name="surrounder"></param>
    /// <returns>One string containing the items of the list separated by whatever seperator is set to, which is a comma by default.</returns>
    public static string AsString<T>(this List<T> list, string separator = ",", string surrounder = null)
    {
        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < list.Count; i++)
        {
            if (i == list.Count - 1)
            {
                builder.Append($"{surrounder}{list[i]}{surrounder}".Trim());
            }
            else
            {
                builder.Append($"{surrounder}{list[i]}{surrounder}{separator}".Trim() + " ");
            }
        }

        return builder.ToString().Trim();
    }
}