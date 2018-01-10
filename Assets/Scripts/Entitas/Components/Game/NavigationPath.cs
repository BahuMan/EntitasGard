using System.Collections.Generic;
using Entitas;

public class NavigationPathComponent : IComponent
{
    public Stack<HexCellBehaviour> path;
}
