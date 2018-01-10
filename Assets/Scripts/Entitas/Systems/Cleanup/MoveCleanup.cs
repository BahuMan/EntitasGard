using Entitas;

namespace Systems.Cleanup
{
    public class MoveCleanup : ICleanupSystem
    {
        IGroup<GameEntity> _moves;

        public MoveCleanup(Contexts contexts)
        {
            _moves = contexts.game.GetGroup(GameMatcher.Move);
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var m in _moves.GetEntities())
            {
                m.RemoveMove();
            }
        }
    }
}
