namespace ShortestPaths
{
  internal class Link
  {
    public Network Network { get; private set; }
    public Node FromNode { get; private set; }
    public Node ToNode { get; private set; }
    public int Cost { get; private set; }

    public Link(Network network, Node fromNode, Node toNode, int cost)
    {
      Network = network;
      FromNode = fromNode;
      ToNode = toNode;
      Cost = cost;

      FromNode.AddLink(this);
      Network.AddLink(this);
    }
    public override string ToString()
    {
      return $"{FromNode} --> {ToNode} ({Cost})";
    }
  }
}
