namespace Game5.Data.Helper
{
    public static class StringHelper
    {
        public static string ToSnakeCase(this string val)
        {
            for (var i = val.Length - 1; i >= 0; i--)
                if (char.IsUpper(val[i]))
                    val = val.Insert(i, "_");
            return val.ToLower().Trim('_');
        }
    }
}