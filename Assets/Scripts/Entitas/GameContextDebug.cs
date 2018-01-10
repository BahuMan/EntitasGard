using Entitas;

public partial class GameContext
{
    public void Log(string msg)
    {
        GameEntity ge = this.CreateEntity();
        ge.AddDebugMessage(msg);
    }
}
