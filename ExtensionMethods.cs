using System.Text;

namespace BackendCSharp;

public static class Extensions
{
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