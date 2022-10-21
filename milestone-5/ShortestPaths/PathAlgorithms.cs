using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPaths
{
  /// <summary>
  /// Stategy Pattern for Path Algorithms on Networks
  /// </summary>
  interface PathAlgorithm
  {
    void FindPathTree(Network network);
  }

  internal class LabelSetting : PathAlgorithm
  {
    public override string ToString() => "Label Setting";

    public void FindPathTree(Network network) { }
  }

  internal class LabelCorrecting : PathAlgorithm
  {
    public override string ToString() => "Label Correcting";

    public void FindPathTree(Network network) { }
  }
}