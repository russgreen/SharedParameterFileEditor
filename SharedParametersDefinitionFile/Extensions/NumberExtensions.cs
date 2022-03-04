namespace SharedParametersFile.Extensions;

public static class NumberExtensions
{
    public static int? ToNullableInt(this string s)
    {
        int i;
        if (int.TryParse(s, out i)) return i;
        return null;
    }
}
