//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly AutoAttacking autoAttackingComponent = new AutoAttacking();

    public bool isAutoAttacking {
        get { return HasComponent(GameComponentsLookup.AutoAttacking); }
        set {
            if (value != isAutoAttacking) {
                var index = GameComponentsLookup.AutoAttacking;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : autoAttackingComponent;

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

    static Entitas.IMatcher<GameEntity> _matcherAutoAttacking;

    public static Entitas.IMatcher<GameEntity> AutoAttacking {
        get {
            if (_matcherAutoAttacking == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AutoAttacking);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAutoAttacking = matcher;
            }

            return _matcherAutoAttacking;
        }
    }
}
