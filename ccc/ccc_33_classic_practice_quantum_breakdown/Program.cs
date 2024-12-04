using System.Globalization;
using CodingHelper;
using CsvHelper;
using CsvHelper.Configuration;

var r = new InputReader(5, true, " ", false);


foreach (var l in r.GetInputs()) {
    List<FlightId> flightEntries = new();
    Dictionary<int, FlightData> coordinatesByFlightId = new();
    //l.SetOutput();
    var maxTransferRange = l.ReadDouble();
    var flightEntryAmount = l.ReadInt();
    // for loop flightEntryAmount times
    for (int i = 0; i < flightEntryAmount; i++) {
        var flightId = l.ReadInt();
        flightEntries.Add(new FlightId(flightId));
        coordinatesByFlightId[flightId] = new FlightData();
    }

    foreach (var fe in flightEntries) {
        // open a csv file that has the name of the flightId
        var fileName = InputReader.GetCompletePath($"files/usedFlights/{fe.Id}.csv");
        // read the file and get coordinates at the timestamp
        // csv is in the following format
        // startAirport
        // endAirport
        // takeOffTimestamp
        // N
        // timestampOffset,lat,long,altitude (repeats N times)
        bool found = false;
        // if there is a timestamp that is equal to the timestamp of the flight entry print the coordinates
        // if not get timestamp above and below and interpolate
        // get the line of the file that has the timestamp or the two lines that are above and below
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
            HasHeaderRecord = false
        };
        using var reader = new StreamReader(fileName);
        using (var csv = new CsvReader(reader, config)) {
            // get the records but the first 4 lines are different and are single values
            csv.Read();
            var startAirport = csv.GetField<string>(0);
            csv.Read();
            var endAirport = csv.GetField<string>(0);
            csv.Read();
            var takeOffTimestamp = csv.GetField<int>(0);
            csv.Read();
            // add to flight data
            coordinatesByFlightId[fe.Id].StartAirport = startAirport;
            coordinatesByFlightId[fe.Id].EndAirport = endAirport;
            coordinatesByFlightId[fe.Id].TakeOffTimestamp = takeOffTimestamp;
            var N = csv.GetField<int>(0);
            // get the records and add takeOffTimestamp to the timestamp (first value of the record)
            for (int j = 0; j < N; j++) {
                csv.Read();
                var record = csv.GetRecord<Coordinate>();
                record.AbsoluteTimestamp += takeOffTimestamp;
                // add to the dictionary
                coordinatesByFlightId[fe.Id].Coordinates.Add(record);
            }
        }
    }

    foreach (var c in coordinatesByFlightId) {
        Console.WriteLine(c.Value.EndAirport);
    }

}

public record FlightId(int Id);

public class FlightData {
    public string StartAirport { get; set; }
    public string EndAirport { get; set; }
    public int TakeOffTimestamp { get; set; }
    public List<Coordinate> Coordinates { get; set; } = new();
}

public class Coordinate {
    
    public int AbsoluteTimestamp { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
}