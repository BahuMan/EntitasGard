using Entitas;
using Entitas.CodeGeneration.Attributes;

[System.Serializable]
public class LocationComponent: IComponent
{
    public HexCellBehaviour cell;
    [EntityIndex]
    public int cellid;
}

