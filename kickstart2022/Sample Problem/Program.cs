namespace Sample_Problem {
    class Program {
        static void Main(string[] args) {
            var testCases = Convert.ToInt32(Console.ReadLine());

            for (int i = 1; i <= testCases; i++) {
                var kingdom = Console.ReadLine();
                
                var name = ("aeiouAEIOU".IndexOf(kingdom.Last()) >= 0) ? "Alice" : "Bob";
                if (kingdom.Last() == 'y') name = "nobody";

                Console.WriteLine($"Case #{i}: {kingdom} is ruled by {name}.");
            }
        }
    }
}