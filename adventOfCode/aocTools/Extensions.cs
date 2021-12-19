namespace aocTools;

public static class Extensions {
    public static long ToDecimal(this string @this) => Convert.ToInt64(Convert.ToString(Convert.ToInt64(@this, 2), 10));

    public static string CutFromBeginning(this string @this, int length) => @this.Substring(length);
    public static string GetFromBeginning(this string @this, int length) => @this.Substring(0, length);
    public static int ToInt(this char @this) => Convert.ToInt32(@this.ToString());
}