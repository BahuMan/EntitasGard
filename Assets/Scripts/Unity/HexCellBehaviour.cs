using UnityEngine;
using General;

[SelectionBase]
public class HexCellBehaviour : MonoBehaviour {

    [SerializeField, EnumFlag("Passable")]
    public HexPassable _hexPassable;

    [SerializeField]
    public Vector3 cubeCoordinates;

    [SerializeField]
    private Material _highLight;
    private Material _standard;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = this.GetComponentInChildren<Renderer>();
        _standard = _renderer.material;
    }
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

    public void SetHighLight(bool hi)
    {
        _renderer.material = hi ? _highLight : _standard;
    }
}
