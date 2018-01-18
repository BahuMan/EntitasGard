using System.Collections.Generic;
using Entitas;

[Game]
public class NavigationPathComponent : IComponent
{
    public Stack<HexCellBehaviour> path;
}
