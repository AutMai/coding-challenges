using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
class Tournament
{
    public static List<string> LoadFile(string filename = "level4/level4_1.in")
    {
        return File.ReadAllLines(filename).ToList();
    }

    public static string CheckWinner(string styles)
    {
        if (styles.Contains("R") && styles.Contains("P"))
            return "P";
        else if (styles.Contains("R") && styles.Contains("S"))
            return "R";
        else if (styles.Contains("P") && styles.Contains("S"))
            return "S";
        else if (styles[0] == styles[1])
            return styles[0].ToString();
        return "";
    }

    public static string TournamentRound(string fighters)
    {
        var pairs = new List<string>();
        string left = "";

        while (fighters.Length > 0)
        {
            pairs.Add(fighters.Substring(0, 2));
            fighters = fighters.Substring(2);
        }

        foreach (var pair in pairs)
        {
            string roundWinner = CheckWinner(pair);
            left += roundWinner;
        }

        return left;
    }

    public static string CheckTournamentWinners(string fighters)
    {
        while (fighters.Length > 1)
        {
            fighters = TournamentRound(fighters);
        }
        return fighters;
    }

    public static void WriteToFile(List<string> data, string filename = "level3/level3_1.out")
    {
        File.WriteAllLines(filename, data);
    }

    public static List<string> Tournament(List<string> data)
    {
        var rounds = new List<string*/