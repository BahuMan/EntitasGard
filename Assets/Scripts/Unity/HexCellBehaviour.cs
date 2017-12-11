using UnityEngine;
using General;

[SelectionBase]
public class HexCellBehaviour : MonoBehaviour {

    [SerializeField, EnumFlag("Passable")]
    HexPassable _hexPassable;

    [SerializeField]
    int x, y, z;

    public bool CanGo(HexPassable dir)
    {
        return (dir & _hexPassable) == dir;
    }
    public void AllowGo(HexPassable dir)
    {
        _hexPassable |= dir;
    }

    public void BlockGo(HexPassable dir) {
        _hexPassable &= ~dir;
    }
}
