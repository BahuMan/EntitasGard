//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly LocalPlayerComponent localPlayerComponent = new LocalPlayerComponent();

    public bool isLocalPlayer {
        get { return HasComponent(GameComponentsLookup.LocalPlayer); }
        set {
            if (value != isLocalPlayer) {
                var index = GameComponentsLookup.LocalPlayer;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : localPlayerComponent;

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

    static Entitas.IMatcher<GameEntity> _matcherLocalPlayer;

    public static Entitas.IMatcher<GameEntity> LocalPlayer {
        get {
            if (_matcherLocalPlayer == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.LocalPlayer);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherLocalPlayer = matcher;
            }

            return _matcherLocalPlayer;
        }
    }
}
