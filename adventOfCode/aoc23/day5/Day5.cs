using System.Collections.Concurrent;
using aocTools;
using aocTools.Interval;

namespace aoc23.day5;

public class Day5 : AAocDay {
    public Day5() : base(true) {
        ReadInput();
    }

    List<long> _seeds = new();
    List<CategoryMap?> _stages = new();

    private void ReadInput() {
        InputTokens.Remove(1);
        while (InputTokens.JustRead() != "") {
            _seeds.Add(InputTokens.ReadLong());
        }

        InputTokens.Remove(1);
        CategoryMap? map = null;
        while (InputTokens.HasMoreTokens()) {
            if (InputTokens.JustRead() == "") {
                _stages.Add(map);
                map = null;
                InputTokens.Remove(1);
                continue;
            }

            if (map == null) {
                var name = InputTokens.Read().Split("-to-");
                map = new CategoryMap(name[0], name[1]);
                InputTokens.Remove(1);
            }
            else {
                var from = InputTokens.ReadLong();
                var to = InputTokens.ReadLong();
                var range = InputTokens.ReadLong();
                map.Map.Add(new Entry(new Interval(from, from + range), new Interval(to, to + range)));
            }
        }

        _stages.Add(map);
    }

    public override void PuzzleOne() {
        MapSeeds();
    }

    private void MapSeeds() {
        List<long> locations = new();
        // move seed through all maps and get the final value
        foreach (var seed in _seeds) {
            var value = seed;
            foreach (var map in _stages) {
                foreach (var mapEntry in map.Map.Where(mapEntry =>
                             value >= mapEntry.Source.Start && value < mapEntry.Source.End)) {
                    value = mapEntry.Destination.Start + (value - mapEntry.Source.Start);
                    break;
                }
            }

            locations.Add(value);
        }

        Console.WriteLine($"The smallest location is {locations.Min()}");
    }

    public override void PuzzleTwo() {
        var seedRanges = new HashSet<Interval>();
        for (var i = 0; i < _seeds.Count; i += 2) {
            seedRanges.Add(new Interval(_seeds[i], _seeds[i] + _seeds[i + 1]));
        }

        var minLocation = long.MaxValue;
        foreach (var seedRange in seedRanges) {
            // the seed range has to be processed by each map and therefore could be split into multiple ranges
            Console.WriteLine($"The seed range is {seedRange}");
            var rangesToProcess = new Queue<Interval>();
            var rangesProcessed = new HashSet<Interval>();
            rangesProcessed.Add(seedRange);
            foreach (var stage in _stages) {
                rangesToProcess.Clear();
                // add all processed ranges to the queue
                foreach (var processedRange in rangesProcessed) {
                    rangesToProcess.Enqueue(processedRange);
                }

                rangesProcessed.Clear();
                foreach (var map in stage.Map) {
                    // process each range in the queue
                    // add the intersection to the processed ranges
                    // add the remainders to the queue
                    // move to next map after all ranges are tryed against the map // but we need them again later for the next map
                    var rangesToProcessNext = new Queue<Interval>();
                    while (rangesToProcess.Count > 0) {
                        var range = rangesToProcess.Dequeue();
                        var intersection = map.Source.Intersection(range);
                        if (intersection == null) {
                            rangesToProcessNext.Enqueue(range);
                            continue;
                        }

                        rangesProcessed.Add(ToDestinationInterval(intersection, map));
                        var remainders = range.ExceptMultiSide(intersection);
                        foreach (var remainder in remainders) {
                            rangesToProcessNext.Enqueue(remainder);
                        }
                    }

                    rangesToProcess = rangesToProcessNext;
                }

                // at this point the rangesToProcess contains the ranges that are not in any map and therefore should added to the processed ranges as they are
                while (rangesToProcess.Count > 0) {
                    rangesProcessed.Add(rangesToProcess.Dequeue());
                }

                //Console.WriteLine($"The ranges processed are {string.Join(", ", rangesProcessed)}");
            }
            // add minimum of the processed ranges to the minLocation
            minLocation = Math.Min(minLocation, rangesProcessed.Min(x => x.Start));
        }

        Console.WriteLine($"The smallest location is {minLocation}");
    }


    /*public override void PuzzleTwo() { REVERSE TRY
        // reverse the maps
        _maps.Reverse();
        var maxLocation = _maps[0].Map.Keys.Max(x => x.Item1 + _maps[0].Map[x]);
        Console.WriteLine(maxLocation);
        for (long i = 0; i < maxLocation; i++) {
            var value = i;
            foreach (var map in _maps) {
                foreach (var mapEntry in map.Map) {
                    if (value >= mapEntry.Key.Item1 && value < mapEntry.Key.Item1 + mapEntry.Value) {
                        value = mapEntry.Key.Item2 + (value - mapEntry.Key.Item1);
                        break;
                    }
                }
            }

            // check if the value is in seed ranges (two ints in _seeds is a range)
            var inRange = false;
            for (var j = 0; j < _seeds.Count; j += 2) {
                if (value < _seeds[j] || value >= _seeds[j] + _seeds[j + 1]) continue;
                inRange = true;
                break;
            }

            if (inRange) {
                Console.WriteLine($"{i} found");
                return;
            }
            else {
                Console.WriteLine($"{i} not found");
            }
        }
    }*/
    
    private Interval ToDestinationInterval(Interval source, Entry map) {
        return new Interval(map.Destination.Start + (source.Start - map.Source.Start), map.Destination.Start + (source.End - map.Source.Start));
    }
}

class CategoryMap {
    public string FromLabel { get; set; }
    public string ToLabel { get; set; }

    public HashSet<Entry> Map { get; set; } = new();

    public CategoryMap(string fromLabel, string toLabel) {
        FromLabel = fromLabel;
        ToLabel = toLabel;
    }
}

record Entry(Interval Destination, Interval Source);