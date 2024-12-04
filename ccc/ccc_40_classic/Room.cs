using System.Numerics;
using System.Text;

namespace ccc_40_classic;

public class Room {
    public int Width { get; set; }
    public int Height { get; set; }
    public int DeskAmount { get; set; }

    public const int DeskWidth = 2;
    public const int DeskHeight = 1;

    public Room(int width, int height, int deskAmount) {
        Width = width;
        Height = height;
        DeskAmount = deskAmount;
        ArrangeDesks();
    }

    public DeskCollection ArrangedDesks { get; set; } = new DeskCollection();
    public int CurrentId { get; set; } = 1;

    public int? CheckpointX { get; set; } = 0;
    public int? CheckpointY { get; set; } = 0;
    
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;

    public StateMemory StateMemory { get; set; } = new StateMemory();
    
    public bool PrimaryRotation { get; set; } = true;
    
    private State CurrentState => new State(ArrangedDesks, X, Y);

    public void ArrangeDesks() {
        for (Y = CheckpointY ?? 0; Y < Height; Y++) {
            CheckpointY = null;
            for (X = CheckpointX ?? 0; X < Width; X++) {
                CheckpointX = null;
                
                var desk = TryDesk(true);
                if (desk == null) {
                    continue;
                }
                
                ArrangedDesks.Add(desk);
                
                if (StateMemory.Contains(CurrentState)) {
                    // check other way around
                    ArrangedDesks.Remove(desk);
                    
                    var desk2 = TryDesk(false);
                    if (desk2 == null) {
                        continue;
                    }
                
                    ArrangedDesks.Add(desk2);
                
                    if (StateMemory.Contains(CurrentState)) {
                        ArrangedDesks.Remove(desk2);
                        
                        // add a state to the memory
                        StateMemory.Add(CurrentState);
                        
                        // return to the last state
                        PrimaryRotation = !ArrangedDesks.Last().Horizontal;
                        ArrangedDesks.Remove(ArrangedDesks.Last());
                        CurrentId--;
                        CheckpointX = Convert.ToInt32(ArrangedDesks.Last().Position.X);
                        CheckpointY = Convert.ToInt32(ArrangedDesks.Last().Position.Y);
                        ArrangeDesks();
                    }
                }
                

                if (ArrangedDesks.Count() == DeskAmount) {
                    return;
                }
            }
        }

        if (DeskAmount == ArrangedDesks.Count()) {
            return;
        }
        else {
            // store this as a failed state
            // and return to latest state
            StateMemory.Add(new State(ArrangedDesks, X, Y));
            PrimaryRotation = !ArrangedDesks.Last().Horizontal;
            ArrangedDesks.Remove(ArrangedDesks.Last());
            CurrentId--;
            CheckpointX = Convert.ToInt32(ArrangedDesks.Last().Position.X);
            CheckpointY = Convert.ToInt32(ArrangedDesks.Last().Position.Y);
            ArrangeDesks();
        }
    }


    public Desk? TryDesk(bool primaryOrientation) {
        var desk = TryDesk2(primaryOrientation);
        if (desk == null) {
            desk = TryDesk2(!primaryOrientation);
        }

        CurrentId++;
        return desk;
    }

    public Desk? TryDesk2(bool horizontal) {
        // create a new desk
        Desk desk;
        desk = horizontal 
            ? new Desk(DeskWidth, DeskHeight, new Vector2(X, Y), true) 
            : new Desk(DeskWidth, DeskHeight, new Vector2(X, Y), false);

        // check if there is enough space for the desk
        if (desk.DeskPositions.Any(p => p.X >= Width || p.Y >= Height)) {
            ArrangedDesks.Remove(desk);
            return null;
        }

        // check if the desk collides with any other desk
        if (ArrangedDesks.Any(d => d.CollidesWithGap(desk))) {
            ArrangedDesks.Remove(desk);
            return null;
        }
        
        return desk;
    }

    public string GetRoomMap() {
        // display each tile of desk as their id but leave spaces in each tile for the whole room
        var sb = new StringBuilder();

        for (var y = 0; y < Height; y++) {
            for (var x = 0; x < Width; x++) {
                var desk = ArrangedDesks.FirstOrDefault(d => d.DeskPositions.Any(p => p.X == x && p.Y == y));

                if (desk != null) {
                    sb.Append("X");
                }
                else {
                    sb.Append(".");
                }

                // if (x != Width - 1) {
                //     sb.Append(" ");
                // }
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}