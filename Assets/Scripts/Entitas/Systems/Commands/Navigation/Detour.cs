using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Systems.Command.Navigation
{
    public class Detour : ReactiveSystem<GameEntity>
    {
        GameContext _game;
        HexGridBehaviour _grid;

        public Detour(Contexts contexts) : base(contexts.game)
        {
            _game = contexts.game;
            _grid = GameObject.FindObjectOfType<HexGridBehaviour>();
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.NavigationBlocked);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasLocation && entity.hasNavigationTarget && entity.hasNavigationPath && entity.isNavigationBlocked;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var unit in entities)
            {
                HexCellBehaviour blockedCell = unit.navigationPath.path.Pop();
                GameEntity cell = _game.GetEntityWithID(unit.navigationTarget.targetCellID);
                HexCellBehaviour  targetCell = cell.gameObject.value.GetComponent<HexCellBehaviour>();

                if (blockedCell == targetCell) //can't get closer
                {
                    unit.RemoveNavigationTarget();
                }
                else
                {
                    //force calculation of new path with current traverse costs:
                    unit.ReplaceNavigationTarget(cell.iD.value);
                    unit.isNavigationBlocked = false;
                }
            }
        }
    }
}
