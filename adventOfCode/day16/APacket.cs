namespace day16;

public abstract class APacket {
    protected APacket(long version, long id, OperationPacket? parentPacket) {
        Version = version;
        Id = id;
        ParentPacket = parentPacket;
    }

    public long Version { get; set; }

    public OperationPacket? ParentPacket { get; set; }
    public long Id { get; set; }

    public virtual long GetVersionNr() {
        return 0;
    }

    public virtual long EvaluateValue() {
        return 0;
    }
}