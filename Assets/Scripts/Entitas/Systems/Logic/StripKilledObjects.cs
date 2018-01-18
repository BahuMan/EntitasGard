using System.Collections.Generic;
using Entitas;

namespace Systems.Logic
{
    public class StripKilledObjects : ReactiveSystem<GameEntity>
    {
        public StripKilledObjects(Contexts contexts): base(contexts.game)
        {
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                e.isSelected = false;
                e.isSelectable = false;
                e.isWeaponAimed = false;
                if (e.hasAttackTarget) e.RemoveAttackTarget();
                if (e.hasMove) e.RemoveMove();
            }
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isKilled;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Killed);
        }
    }
}
