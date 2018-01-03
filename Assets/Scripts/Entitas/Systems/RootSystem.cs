using Entitas;

public class RootSystem : Feature
{
    public RootSystem(Contexts contexts): base("Root System")
    {
        //make sure that every game entity created also gets an ID component:
        contexts.game.OnEntityCreated += AddId;

        Add(new InitHelloWorld(contexts));
        Add(new Systems.Input.MouseClickInput(contexts));
        //Add(new Systems.Input.MouseSelectInput(contexts));
        //Add(new Systems.Input.MouseMoveInput(contexts));
        Add(new Systems.View.ShowSelectionBox(contexts));
        Add(new Systems.View.LogDebugMessageSystem(contexts));
    }

    //this method gets called whenever a game entity has been created
    private void AddId(IContext context, IEntity entity)
    {
        (entity as IID).AddID(entity.creationIndex);
    }
}
