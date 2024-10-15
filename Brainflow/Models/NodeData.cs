namespace Brainflow;

public class NodeData{
    public string id { get; set; }
    public Position position { get; set; }
    public Data data { get; set; }
    public string target { get; set; }
    public string type { get; set; }
}
public class Position{
    public float x { get; set; }
    public float y { get; set; }
}
public class Data {
    public string label { get; set; }
}
