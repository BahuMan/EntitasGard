using Entitas;

[Game]
[System.Serializable]
public class NavigableComponent : IComponent
{
    public float turnRate; //degrees per second
    public float moveRate; //Unity units (meters) per second
}
