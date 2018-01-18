using Entitas;

public class DamageCleanup : ICleanupSystem
{
    IGroup<GameEntity> _damaged;

    public DamageCleanup(Contexts contexts)
    {
        _damaged = contexts.game.GetGroup(GameMatcher.Damage);
    }

    public void Cleanup()
    {
        foreach(var e in _damaged.GetEntities())
        {
            e.RemoveDamage();
        }
    }
}
