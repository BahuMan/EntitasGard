using Entitas;

public class InitHelloWorld : IInitializeSystem
{

    private GameContext _context;

    public InitHelloWorld(Contexts contexts)
    {
        _context = contexts.game;
    }

    public void Initialize()
    {
        _context.CreateEntity().AddDebugMessage("Hello World!");
    }
}
