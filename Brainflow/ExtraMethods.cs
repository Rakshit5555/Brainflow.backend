namespace Brainflow;

public class ExtraMethods
{
    //     public Task SendShapeUpdate(string roomId, ShapeData shape)
    // {
    //     if (!rooms.ContainsKey(roomId))
    //     {
    //         rooms[roomId] = new Room { id = roomId };
    //     }

    //     var room = rooms[roomId];
    //     var existingShape = room.Shapes.Find(s => s.id == shape.id);
    //     if (existingShape != null)
    //     {
    //         room.Shapes.Remove(existingShape);
    //     }
    //     room.Shapes.Add(shape);

    //     return Clients.Group(roomId).SendAsync("ReceiveShapeUpdate", shape);
    // }

    // public Task SendTextUpdate(string roomId, TextData text)
    // {
    //     if (!rooms.ContainsKey(roomId))
    //     {
    //         rooms[roomId] = new Room { id = roomId };
    //     }

    //     var room = rooms[roomId];
    //     var existingText = room.Texts.Find(t => t.id == text.id);
    //     if (existingText != null)
    //     {
    //         room.Texts.Remove(existingText);
    //     }
    //     room.Texts.Add(text);

    //     return Clients.Group(roomId).SendAsync("ReceiveTextUpdate", text);
    // }

}

public class ShapeData
{
    public string id { get; set; }
    public string type { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public float width { get; set; }
    public float height { get; set; }
    public string stroke { get; set; }
    public float strokeWidth { get; set; }
    public string text { get; set; }
}

public class TextData
{
    public string id { get; set; }
    public string content { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public string color { get; set; }
    public string fontSize { get; set; }
}



