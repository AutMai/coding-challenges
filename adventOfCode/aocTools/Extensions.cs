namespace aocTools;

public static class Extensions {
    public static long ToDecimal(this string @this) => Convert.ToInt64(Convert.ToString(Convert.ToInt64(@this, 2), 10));

    public static string CutFromBeginning(this string @this, int length) => @this.Substring(length);
    public static string GetFromBeginning(this string @this, int length) => @this.Substring(0, length);
    public static int ToInt(this char @this) => Convert.ToInt32(@this.ToString());
    public static int ToInt(this string @this) => Convert.ToInt32(@this);

    public static TokenList ToTokenList(this List<string> @this) => new TokenList(@this);

    public static List<string> RemoveEmptyTokens(this List<string> @this) =>
        @this.Where(x => !string.IsNullOrEmpty(x)).ToList();

    public static int Kgv(int a, int b) {
        return (a / Ggt(a, b)) * b;
    }

    public static int Ggt(int a, int b) {
        while (b != 0) {
            int temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    static int gcd(int n1, int n2) {
        if (n2 == 0) {
            return n1;
        }
        else {
            return gcd(n2, n1 % n2);
        }
    }

    public static int lcm(int[] numbers) {
        return numbers.Aggregate((S, val) => S * val / gcd(S, val));
    }
}