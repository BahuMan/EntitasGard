//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly CanBuildBarracks canBuildBarracksComponent = new CanBuildBarracks();

    public bool isCanBuildBarracks {
        get { return HasComponent(GameComponentsLookup.CanBuildBarracks); }
        set {
            if (value != isCanBuildBarracks) {
                var index = GameComponentsLookup.CanBuildBarracks;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : canBuildBarracksComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
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

    static Entitas.IMatcher<GameEntity> _matcherCanBuildBarracks;

    public static Entitas.IMatcher<GameEntity> CanBuildBarracks {
        get {
            if (_matcherCanBuildBarracks == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CanBuildBarracks);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCanBuildBarracks = matcher;
            }

            return _matcherCanBuildBarracks;
        }
    }
}
