using System.Collections.Generic;
using Entitas;

namespace Systems.Commands.MoveOneHex
{
    public class FindNeighborToMove : ReactiveSystem<GameEntity>
    {

        public FindNeighborToMove(Contexts contexts) : base(contexts.game)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.NavigationTarget);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasNavigationTarget;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach(var unit in entities)
            {

            }
        }
    }
}
