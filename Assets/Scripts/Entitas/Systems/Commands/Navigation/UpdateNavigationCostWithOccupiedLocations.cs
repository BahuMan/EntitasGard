using System.Collections.Generic;
using Entitas;

namespace Systems.Command.Navigation
{
    public class UpdateNavigationCostWithOccupiedLocations : ReactiveSystem<GameEntity>
    {
        public UpdateNavigationCostWithOccupiedLocations(Contexts contexts): base(contexts.game)
        {

        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Location, GameMatcher.LeaveCell));
        }

        protected override bool Filter(GameEntity entity)
        {
            return (entity.hasLocation || entity.hasLeaveCell);
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach(var unit in entities)
            {
                if (unit.hasLeaveCell) unit.leaveCell.cell.traverseCost = 1;
                if (unit.hasLocation) unit.location.cell.traverseCost = 1000;
            }
        }
    }
}
