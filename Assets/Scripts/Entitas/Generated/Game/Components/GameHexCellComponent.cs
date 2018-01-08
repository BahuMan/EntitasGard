//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public HexCellComponent hexCell { get { return (HexCellComponent)GetComponent(GameComponentsLookup.HexCell); } }
    public bool hasHexCell { get { return HasComponent(GameComponentsLookup.HexCell); } }

    public void AddHexCell(float newWorldx, float newWorldy, float newWorldz, float newCubex, float newCubey, float newCubez) {
        var index = GameComponentsLookup.HexCell;
        var component = CreateComponent<HexCellComponent>(index);
        component.worldx = newWorldx;
        component.worldy = newWorldy;
        component.worldz = newWorldz;
        component.cubex = newCubex;
        component.cubey = newCubey;
        component.cubez = newCubez;
        AddComponent(index, component);
    }

    public void ReplaceHexCell(float newWorldx, float newWorldy, float newWorldz, float newCubex, float newCubey, float newCubez) {
        var index = GameComponentsLookup.HexCell;
        var component = CreateComponent<HexCellComponent>(index);
        component.worldx = newWorldx;
        component.worldy = newWorldy;
        component.worldz = newWorldz;
        component.cubex = newCubex;
        component.cubey = newCubey;
        component.cubez = newCubez;
        ReplaceComponent(index, component);
    }

    public void RemoveHexCell() {
        RemoveComponent(GameComponentsLookup.HexCell);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherHexCell;

    public static Entitas.IMatcher<GameEntity> HexCell {
        get {
            if (_matcherHexCell == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.HexCell);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherHexCell = matcher;
            }

            return _matcherHexCell;
        }
    }
}