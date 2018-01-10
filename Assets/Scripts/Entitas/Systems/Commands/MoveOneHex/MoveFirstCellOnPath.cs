using System.Collections.Generic;
using Entitas;

public class MoveFirstCellOnPath : ReactiveSystem<GameEntity>
{
    public MoveFirstCellOnPath(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.NavigationTarget);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasLocation && entity.hasNavigationTarget;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var unit in entities)
        {
            HexCellBehaviour curCell = unit.location.cell;

        }
    }
}
