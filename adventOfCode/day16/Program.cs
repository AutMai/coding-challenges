using System.Threading.Channels;
using aocTools;
using day16;

var hexString = aocTools.Helper.ReadFile("input.txt");

string binaryString = String.Join(String.Empty,
    hexString.Select(
        c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
    )
);

// 6 % 4 = 2

// 00111000000000000110111101000101001010010001001000000000
var currentPacket = new OperationPacket(000, 000, null);
OperationPacket rootPacket = null;

try {
    CreatePacket(ref binaryString);
}
catch (Exception e) {
    Console.WriteLine();
}

Console.WriteLine(rootPacket.GetVersionNr());

Console.WriteLine(rootPacket.EvaluateValue());


void CreatePacket(ref string packetString) {
    var versionNr = packetString.GetFromBeginning(3).ToDecimal();
    packetString = packetString.CutFromBeginning(3);
    var id = packetString.GetFromBeginning(3).ToDecimal();
    packetString = packetString.CutFromBeginning(3);

    if (id == 4)
        CreateValuePacket(versionNr, id, ref packetString);
    else
        CreateOperatorPacket(versionNr, id, ref packetString);
}

void CreateOperatorPacket(long versionNr, long id, ref string packetString) {
    currentPacket.ChildPackets.Add(currentPacket = new OperationPacket(versionNr, id, currentPacket));

    rootPacket ??= currentPacket;

    var subpacketsBitLength = (packetString.GetFromBeginning(1) == "0") ? 15 : 11;
    packetString = packetString.CutFromBeginning(1);

    if (subpacketsBitLength == 15) {
        var subpacketsLength = Convert.ToInt32(packetString.GetFromBeginning(subpacketsBitLength).ToDecimal());
        packetString = packetString.CutFromBeginning(subpacketsBitLength);

        var subpackets = packetString.GetFromBeginning(subpacketsLength);
        packetString = packetString.CutFromBeginning(subpacketsLength);

        while (subpackets.Length > 0) {
            CreatePacket(ref subpackets);
        }

        currentPacket = currentPacket.ParentPacket;
    }
    else {
        var subpacketsAmount = packetString.GetFromBeginning(subpacketsBitLength).ToDecimal();
        packetString = packetString.CutFromBeginning(subpacketsBitLength);

        for (long i = 0; i < subpacketsAmount; i++) {
            CreatePacket(ref packetString);
        }

        currentPacket = currentPacket.ParentPacket;
    }
}

void CreateValuePacket(long versionNr, long id, ref string packetString) {
    var valueStr = "";


    while (true) {
        valueStr += packetString.Substring(1, 4);
        if (packetString[0] == '0') {
            packetString = packetString.CutFromBeginning(5);
            break;
        }

        packetString = packetString.CutFromBeginning(5);
    }

    currentPacket.ChildPackets.Add(new ValuePacket(versionNr, id, valueStr.ToDecimal(), currentPacket));
    //CreatePacket(packetString);
}