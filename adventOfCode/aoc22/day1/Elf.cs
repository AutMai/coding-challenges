namespace aoc22.day1; 

public class Elf {
    public List<int> Calories { get; set; } = new();
    
    public int CalorieCount => Calories.Sum();
}