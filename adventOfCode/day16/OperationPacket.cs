namespace day16;

public class OperationPacket : APacket {
    public List<APacket> ChildPackets { get; set; }


    public override long GetVersionNr() {
        long childVersionSum = ChildPackets.Sum(p => p.GetVersionNr());
        return Version + childVersionSum;
    }

    public OperationPacket(long version, long id, OperationPacket? parentPacket) : base(version, id, parentPacket) {
        ChildPackets = new List<APacket>();
    }

    public override long EvaluateValue() {
        var values = new List<long>();
        foreach (var child in ChildPackets) {
            values.Add(child.EvaluateValue());
        }

        switch (Id) {
            case 0:
                return values.Sum();
            case 1:
                return values.Aggregate((a, b) => a * b);
            case 2:
                return values.Min();
            case 3:
                return values.Max();
            case 5:
                return (values[0] > values[1]) ? 1 : 0;
            case 6:
                return (values[0] < values[1]) ? 1 : 0;
            case 7:
                return (values[0] == values[1]) ? 1 : 0;
        }

        return 0;
    }
}