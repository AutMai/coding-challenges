using CodingHelper;

var r = new InputReader(3, true, ",", false);


foreach (var l in r.GetInputs()) {
    List<FlightEntry> flightEntries = new();
    var airportRoutes = new Dictionary<string, int>();
    l.SetOutput();
    var flightEntryAmount = l.ReadInt();
    // for loop flightEntryAmount times
    for (int i = 0; i < flightEntryAmount; i++) {
        var latitude = l.ReadDouble();
        var longitude = l.ReadDouble();
        var altitude = l.ReadDouble();

        flightEntries.Add(new FlightEntry(latitude, longitude, altitude));
    }

    foreach (var fe in flightEntries) {
        var ecef = Helpers.FlightEntryToECEF(fe);
        Console.WriteLine($"{ecef.X} {ecef.Y} {ecef.Z}");
    }
}

public static class Helpers {
    public static ECEF FlightEntryToECEF(FlightEntry flightEntry) {
        var latitude = flightEntry.Latitude;
        var longitude = flightEntry.Longitude;
        var altitude = flightEntry.Altitude;
        double radius = 6371000.0; // Earth's radius in meters

        // convert latitude and longitude to radians
        double lat_rad = latitude * Math.PI / 180.0;
        double lon_rad = longitude * Math.PI / 180.0;

        // calculate ECEF coordinates
        double x = (radius + altitude) * Math.Cos(lat_rad) * Math.Cos(lon_rad);
        double y = (radius + altitude) * Math.Cos(lat_rad) * Math.Sin(lon_rad);
        double z = (radius + altitude) * Math.Sin(lat_rad);
        return new ECEF(x, y, z);
    }
}

public record FlightEntry(double Latitude, double Longitude, double Altitude);

public record ECEF(double X, double Y, double Z);