namespace day16;

public class ValuePacket : APacket {
    public long Value { get; set; }

    public override long GetVersionNr() {
        return Version;
    }

    public ValuePacket(long version, long id, long value, OperationPacket? parentPacket) :
        base(version, id, parentPacket) {
        Value = value;
    }

    public override long EvaluateValue() {
        return Value;
    }
}