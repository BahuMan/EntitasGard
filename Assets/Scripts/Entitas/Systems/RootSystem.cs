using Entitas;

public class RootSystem : Feature
{
    public RootSystem(Contexts contexts): base("Root System")
    {
        //make sure that every game entity created also gets an ID component:
        contexts.game.OnEntityCreated += AddId;

        Add(new InitHelloWorld(contexts));
        Add(new Systems.Input.MouseInput(contexts));
        Add(new Systems.Selection.ClickSelect(contexts));

        Add(new Systems.Command.NavigationCommand(contexts));
        Add(new Systems.Command.CreateNavigationPath(contexts));

        Add(new Systems.View.ShowEnRoute(contexts));
        Add(new Systems.View.ShowSelectionBox(contexts));
        Add(new Systems.View.SelectionView(contexts));
        //Add(new Systems.View.MouseHoverDebug(contexts));
        Add(new Systems.View.LogDebugMessageSystem(contexts));
    }

    //this method gets called whenever a game entity has been created
    private void AddId(IContext context, IEntity entity)
    {
        (entity as IID).AddID(entity.creationIndex);
    }
}
