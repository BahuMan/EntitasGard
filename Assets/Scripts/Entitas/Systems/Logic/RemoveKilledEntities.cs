using Entitas;

namespace Systems.Logic
{
    public class RemoveKilledEntities : ICleanupSystem
    {
        private IGroup<GameEntity> _killedEntities;

        public RemoveKilledEntities(Contexts contexts)
        {
            _killedEntities = contexts.game.GetGroup(GameMatcher.Killed);
        }

        public void Cleanup()
        {
            foreach (var dead in _killedEntities.GetEntities())
            {
                dead.Destroy();
            }
        }
    }
}
