using Entitas;

public class RootSystem : Feature
{
    public RootSystem(Contexts contexts): base("Root System")
    {
        Add(new InitHelloWorld(contexts));
        Add(new Systems.View.LogDebugMessageSystem(contexts));
    }
}
