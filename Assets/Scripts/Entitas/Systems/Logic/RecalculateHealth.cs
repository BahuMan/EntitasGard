using System.Collections.Generic;
using Entitas;

namespace Systems.Logic
{
    public class RecalculateHealth : ReactiveSystem<GameEntity>
    {
        public RecalculateHealth(Contexts contexts) : base(contexts.game)
        {

        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var unit in entities)
            {
                unit.ReplaceHealth(unit.health.value - unit.damage.damage);
                if (unit.health.value < 0) unit.isKilled = true;
            }
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasHealth && entity.hasDamage;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Damage);
        }
    }
}
