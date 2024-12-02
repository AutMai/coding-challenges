using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;

namespace aoc23.day20;

public class Day20 : AAocDay {
    Dictionary<string, AModule> _modules = new();

    public Day20() {
        foreach (var token in InputTokens) {
            var module = token.Split(" ")[0];
            if (module == "broadcaster") {
                _modules.Add(module, new BroadcasterModule(module));
            }

            var moduleType = module[0];
            var moduleName = module[1..];
            switch (moduleType) {
                case '%':
                    _modules.Add(moduleName, new FlipFlopModule(moduleName));
                    break;
                case '&':
                    _modules.Add(moduleName, new ConjunctionModule(moduleName));
                    break;
            }
        }

        while (InputTokens.HasMoreTokens()) {
            var moduleName = InputTokens.JustRead().Split(" ")[0];
            if (moduleName != "broadcaster") moduleName = moduleName[1..];

            var module = _modules[moduleName];
            var values = InputTokens.Read().Split("-> ")[1];
            module.DestinationModules = new List<AModule>();
            foreach (var value in values.Split(", ")) {
                module.DestinationModules.Add(_modules[value]);
            }
        }
    }

    public override void PuzzleOne() {
        var broadcaster = (BroadcasterModule)_modules["broadcaster"];
        broadcaster.Run(broadcaster, false);
        var result = broadcaster.DestinationModules[0];
        Console.WriteLine(result);
    }

    public override void PuzzleTwo() {
    }
}

abstract class AModule {
    protected AModule(string name) {
        Name = name;
    }

    public string Name { get; set; }
    public List<AModule> DestinationModules { get; set; }

    public abstract void Run(AModule source, bool pulse);

    public override string ToString() {
        return Name;
    }
}

class FlipFlopModule : AModule {
    public FlipFlopModule(string name) : base(name) {
    }

    public bool CurrentStatus { get; set; }

    public override void Run(AModule source, bool pulse) {
        if (pulse) {
            CurrentStatus = !CurrentStatus;
        }

        DestinationModules.ForEach(module => module.Run(this, CurrentStatus));

        // return delegates to the methods
        var x = new List<Func<AModule, bool, object>>();
        foreach (var module in DestinationModules) {
            //x.Add(module.Run); // ?
        }
    }
}

class ConjunctionModule : AModule {
    public ConjunctionModule(string name) : base(name) {
    }

    public Dictionary<AModule, bool> InputModuleMemory { get; set; } = new();

    public override void Run(AModule source, bool pulse) {
        InputModuleMemory.TryAdd(source, pulse);
        if (InputModuleMemory.Count < 2) {
            // the pulse gets inverted
            DestinationModules.ForEach(module => module.Run(this, !pulse));
        }
        else {
            // if all values are the same, send true
            var result = InputModuleMemory.Values.All(value => value);
            DestinationModules.ForEach(module => module.Run(this, result));
        }
    }
}

class BroadcasterModule : AModule {
    public BroadcasterModule(string name) : base(name) {
    }

    public override void Run(AModule source, bool pulse) {
        DestinationModules.ForEach(module => module.Run(this, pulse));
    }
}

class ButtonModule : AModule {
    public ButtonModule(string name) : base(name) {
    }

    public override void Run(AModule source, bool pulse) {
        DestinationModules.ForEach(module => module.Run(this, true));
    }
}