//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public WeaponRotation weaponRotation { get { return (WeaponRotation)GetComponent(GameComponentsLookup.WeaponRotation); } }
    public bool hasWeaponRotation { get { return HasComponent(GameComponentsLookup.WeaponRotation); } }

    public void AddWeaponRotation(float newRy) {
        var index = GameComponentsLookup.WeaponRotation;
        var component = CreateComponent<WeaponRotation>(index);
        component.ry = newRy;
        AddComponent(index, component);
    }

    public void ReplaceWeaponRotation(float newRy) {
        var index = GameComponentsLookup.WeaponRotation;
        var component = CreateComponent<WeaponRotation>(index);
        component.ry = newRy;
        ReplaceComponent(index, component);
    }

    public void RemoveWeaponRotation() {
        RemoveComponent(GameComponentsLookup.WeaponRotation);
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

    static Entitas.IMatcher<GameEntity> _matcherWeaponRotation;

    public static Entitas.IMatcher<GameEntity> WeaponRotation {
        get {
            if (_matcherWeaponRotation == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.WeaponRotation);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherWeaponRotation = matcher;
            }

            return _matcherWeaponRotation;
        }
    }
}