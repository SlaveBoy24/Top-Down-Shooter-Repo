using System.Text;

public static class AdditionalMethods
{
    public static string CamelToSnake(this string camelCaseStr)
    {
        if (string.IsNullOrEmpty(camelCaseStr))
            return camelCaseStr;

        StringBuilder snakeCaseStr = new StringBuilder();
        snakeCaseStr.Append(char.ToLower(camelCaseStr[0]));

        for (int i = 1; i < camelCaseStr.Length; i++)
        {
            if (char.IsUpper(camelCaseStr[i]))
            {
                snakeCaseStr.Append('_');
                snakeCaseStr.Append(char.ToLower(camelCaseStr[i]));
            }
            else
            {
                snakeCaseStr.Append(camelCaseStr[i]);
            }
        }

        return snakeCaseStr.ToString();
    }
}