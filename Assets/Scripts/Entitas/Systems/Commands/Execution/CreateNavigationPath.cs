using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Systems.Command.Execution
{
    public class CreateNavigationPath : ReactiveSystem<GameEntity>
    {

        private GameContext _game;
        private HexGridBehaviour _grid;

        public CreateNavigationPath(Contexts contexts) : base(contexts.game)
        {
            _game = contexts.game;
            _grid = GameObject.FindObjectOfType<HexGridBehaviour>();

        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.NavigationTarget.AddedOrRemoved());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasNavigationTarget || (entity.hasNavigationPath && !entity.hasNavigationTarget);
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var unit in entities)
            {
                if (!unit.hasNavigationTarget)
                {
                    if (unit.hasNavigationPath) unit.RemoveNavigationPath();
                }
                else
                {
                    HexCellBehaviour fromCell = unit.location.cell;

                    GameEntity toCellEntity = _game.GetEntityWithID(unit.navigationTarget.targetCellID);
                    HexCellBehaviour toCell = toCellEntity.gameObject.value.GetComponent<HexCellBehaviour>();

                    Stack<HexCellBehaviour> p = _grid.FindPath(fromCell, toCell);
                    unit.ReplaceNavigationPath(p);
                }
            }
        }
    }
}
