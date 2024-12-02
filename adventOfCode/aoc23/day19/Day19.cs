using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;

namespace aoc23.day19;

public class Day19 : AAocDay {
    List<Part> parts = new();
    WorkflowProcessor workflowProcessor = new();

    public Day19() {
        while (InputTokens.JustRead() != "") {
            var line = InputTokens.Read();
            var workflow = new Workflow();
            workflow.Name = line.Split("{")[0];
            workflow.Rules = new List<Rule>();
            var rules = line.Split("{")[1].Split("}")[0].Split(",");
            foreach (var rule in rules) {
                workflow.Rules.Add(new Rule() { R = rule });
            }

            workflowProcessor.workflows.Add(workflow.Name, workflow);
        }

        InputTokens.Remove(1);

        while (InputTokens.HasMoreTokens()) {
            var Part = new Part();
            var line = InputTokens.Read();
            line = line.Replace("=", "").Replace("{", "").Replace("}", "");
            var values = line.Split(",");
            Part.XRating = int.Parse(values[0][1..]);
            Part.MRating = int.Parse(values[1][1..]);
            Part.ARating = int.Parse(values[2][1..]);
            Part.SRating = int.Parse(values[3][1..]);
            parts.Add(Part);
        }
    }

    public override void PuzzleOne() {
        var x = parts.Sum(p => workflowProcessor.InitProcess(p));

        Console.WriteLine(x);
    }

    public override void PuzzleTwo() {
        parts = new List<Part>();
        // fill parts with four ratings (x, m, a, s) can have an integer value ranging from a minimum of 1 to a maximum of 4000. Of all possible distinct combinations
        for (var x = 1; x <= 4000; x++) {
            for (var m = 1; m <= 4000; m++) {
                for (var a = 1; a <= 4000; a++) {
                    for (var s = 1; s <= 4000; s++) {
                        parts.Add(new Part() { XRating = x, MRating = m, ARating = a, SRating = s });
                    }
                }
            }
        }

        Console.WriteLine(parts.Count);
    }
}

public class WorkflowProcessor {
    public Dictionary<string, Workflow> workflows = new();

    public int InitProcess(Part part) {
        var res = Process(part, workflows["in"]);
        if (res == "A") {
            return part.ARating + part.MRating + part.SRating + part.XRating;
        }

        return 0;
    }

    public string Process(Part part, Workflow workflow) {
        var res = "";
        foreach (var r in workflow.Rules) {
            res = r.Evaluate(part, this);
            if (res != "") {
                break;
            }
        }

        return res;
    }
}

public class Workflow {
    public string Name { get; set; }
    public List<Rule> Rules { get; set; }
}

public class Rule {
    public string R { get; set; }

    public string Category() {
        if (R.Contains(':')) {
            return R[0].ToString();
        }

        return R;
    }

    public string Operator() => R[1].ToString();
    public string Value() => R[2..].Split(":")[0];
    public string Workflow() => R[2..].Split(":")[1];

    public string Evaluate(Part part, WorkflowProcessor workflowProcessor) {
        if (!R.Contains(':')) {
            return Category() is "A" or "R"
                ? Category()
                : workflowProcessor.Process(part, workflowProcessor.workflows[Category()]);
        }

        switch (Category()) {
            case "x" when Operator() == ">" && part.XRating > int.Parse(Value()):
            case "x" when Operator() == "<" && part.XRating < int.Parse(Value()):
            case "m" when Operator() == ">" && part.MRating > int.Parse(Value()):
            case "m" when Operator() == "<" && part.MRating < int.Parse(Value()):
            case "a" when Operator() == ">" && part.ARating > int.Parse(Value()):
            case "a" when Operator() == "<" && part.ARating < int.Parse(Value()):
            case "s" when Operator() == ">" && part.SRating > int.Parse(Value()):
            case "s" when Operator() == "<" && part.SRating < int.Parse(Value()):
                return Workflow() is "A" or "R"
                    ? Workflow()
                    : workflowProcessor.Process(part, workflowProcessor.workflows[Workflow()]);
            default:
                return "";
        }
    }
}

public class Part {
    public int XRating { get; set; }
    public int MRating { get; set; }
    public int ARating { get; set; }
    public int SRating { get; set; }
}