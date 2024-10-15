namespace Brainflow;

public class Room
{
    public string id { get; set; }
    public List<ShapeData> Shapes { get; set; } = new List<ShapeData>();
    public List<TextData> Texts { get; set; } = new List<TextData>();
    public List<string> Notes { get; set; } = new List<string>();
    public List<UserData> Users { get; set; } = new List<UserData>(); // List of users in the room
    public List<NodeData> Nodes { get; set; } = new List<NodeData>(); // List of nodes in the room
    public List<EdgeData> Edges { get; set; } = new List<EdgeData>(); // List of edges in the room
}
