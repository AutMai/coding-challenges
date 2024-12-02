using System.Text;
using aocTools;
using aocTools.Neo4J;

namespace aoc22.day13;

public class Day13 : AAocDay
{
    public override void PuzzleOne()
    {/*
        checke nicht wegen dem
    
        - Compare [2,3,4] vs 4
            - Mixed types; convert right to [4] and retry comparison
            - Compare [2,3,4] vs [4]
            - Compare 2 vs 4
            - Left side is smaller, so inputs are in the right order*/
        
        ReadPackets();

        int sum = 0;
        
        for (int i = 0; i < _packets.Count; i++)
        {
            var p = _packets[i];
            var res = ComparePacketLists(p.Item1, p.Item2);
            if (res == -1)
            {
                sum += i+1;
            }

            Console.WriteLine($"{i}: {res}");
        }

        Console.WriteLine(sum);
    }

    private List<Tuple<PacketList, PacketList>> _packets = new List<Tuple<PacketList, PacketList>>();

    private void ReadPackets()
    {
        while (InputTokens.HasMoreTokens())
        {
            var packet1 = InputTokens.Read();
            var packet2 = InputTokens.Read();
            InputTokens.Remove(1);

            _packets.Add(new Tuple<PacketList, PacketList>(ToPacket(packet1), ToPacket(packet2)));
        }
    }

    private PacketList ToPacket(string packetString)
    {
        PacketList packetList = new PacketList();
        PacketList context = packetList;

        foreach (var letter in packetString)
        {
            switch (letter)
            {
                case '[':
                    var newPacketList = new PacketList { Parent = context };
                    context.Content.Add(newPacketList);
                    context = newPacketList;
                    break;
                case ']':
                    context = context.Parent;
                    break;
                // create a case that handles numbers
                case ',':
                    break;
                default:
                    context.Content.Add(new PacketInt(letter.ToInt()));
                    break;
            }
        }

        packetList = packetList.Content[0] as PacketList ?? new PacketList();

        return packetList;
    }

    private int ComparePacketLists(PacketList packet1, PacketList packet2)
    {
        var minCount = Math.Min(packet1.Content.Count, packet2.Content.Count);

        for (int i = 0; i < minCount; i++)
        {
            var packet1Content = packet1.Content[i];
            var packet2Content = packet2.Content[i];
            int sign = Compare(packet1Content, packet2Content);
            if (sign != 0)
            {
                return sign;
            }
        }

        return Math.Sign(packet1.Content.Count - packet2.Content.Count);
    }

    private int Compare(APacket packet1, APacket packet2)
    {
        return (packet1, packet2) switch
        {
            // if both are packetlists
            (PacketList packetList1, PacketList packetList2) => ComparePacketLists(packetList1, packetList2),
            (PacketInt p1, PacketInt p2) => Math.Sign(p1.Value - p2.Value),
            (PacketList pl1, PacketInt pi2) => ComparePacketLists(pl1,
                new PacketList() { Content = new List<APacket>() { pi2 } }),
            (PacketInt pi1, PacketList pl2) => ComparePacketLists(
                new PacketList() { Content = new List<APacket>() { pi1 } }, pl2),
            _ => throw new Exception("Something went wrong")
        };
    }


    public override void PuzzleTwo()
    {
    }
}

class PacketList : APacket
{
    public List<APacket> Content { get; set; } = new List<APacket>();
}

class PacketInt : APacket
{
    public int Value { get; set; }

    public PacketInt(int value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

class APacket : IPacketType
{
    public PacketList? Parent { get; set; }
}

interface IPacketType
{
    public PacketList? Parent { get; set; }
}