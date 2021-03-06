//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class InputEntity {

    static readonly MouseRightDown mouseRightDownComponent = new MouseRightDown();

    public bool isMouseRightDown {
        get { return HasComponent(InputComponentsLookup.MouseRightDown); }
        set {
            if (value != isMouseRightDown) {
                var index = InputComponentsLookup.MouseRightDown;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : mouseRightDownComponent;

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
public sealed partial class InputMatcher {

    static Entitas.IMatcher<InputEntity> _matcherMouseRightDown;

    public static Entitas.IMatcher<InputEntity> MouseRightDown {
        get {
            if (_matcherMouseRightDown == null) {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.MouseRightDown);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherMouseRightDown = matcher;
            }

            return _matcherMouseRightDown;
        }
    }
}
