using UnityEngine;
using General;
using System;

[SelectionBase]
public class HexCellBehaviour : MonoBehaviour {

    public int traverseCost = 1;

    [SerializeField, EnumFlag("Passable")]
    public HexPassable _hexPassable;

    [SerializeField]
    public Vector3 cubeCoordinates;

#pragma warning disable 0649
    [SerializeField]
    private Material _highLight;
    private Material _standard;
    [SerializeField]
    private Renderer _model;
    [SerializeField]
    private Renderer _N, _S, _NE, _SE, _NW, _SW;
#pragma warning restore 0649

    private void Awake()
    {
        //safeguard default material, for when we want to overwrite it with a highlight:
        _standard = _model.material;
    }

    private void OnValidate()
    {
        MakePassableVisible();
    }

    private void MakePassableVisible()
    {
        _N.gameObject.SetActive(!CanGo(HexPassable.N));
        _NE.gameObject.SetActive(!CanGo(HexPassable.NE));
        _NW.gameObject.SetActive(!CanGo(HexPassable.NW));
        _S.gameObject.SetActive(!CanGo(HexPassable.S));
        _SW.gameObject.SetActive(!CanGo(HexPassable.SW));
        _SE.gameObject.SetActive(!CanGo(HexPassable.SE));
    }

    public bool CanGo(HexPassable dir)
    {
        return (dir & _hexPassable) == dir;
    }

    public void AllowGo(HexPassable dir)
    {
        _hexPassable |= dir;
        MakePassableVisible();
    }

    public void BlockGo(HexPassable dir) {
        _hexPassable &= ~dir;
        MakePassableVisible();
    }

    public void SetHighLight(bool hi)
    {
        _model.material = hi ? _highLight : _standard;
    }

    [ContextMenu("Print Passable enum")]
    public void printPassable()
    {
        Debug.Log("passable = " + Convert.ToString((int)this._hexPassable, 2));

        foreach (HexPassable dir in Enum.GetValues(typeof(HexPassable)))
        {
            Debug.Log("Can go " + dir + " = " + CanGo(dir));
        }
    }
}
