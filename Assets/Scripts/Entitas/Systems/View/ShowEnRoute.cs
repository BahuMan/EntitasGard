using UnityEngine;
using Entitas;
using System.Collections.Generic;

namespace Systems.View
{
    public class ShowEnRoute : ReactiveSystem<GameEntity>
    {

        GameContext _game;
        HexGridBehaviour _grid;

        public ShowEnRoute(Contexts contexts): base(contexts.game)
        {
            _game = contexts.game;
            _grid = GameObject.FindObjectOfType<HexGridBehaviour>();
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Selected.AddedOrRemoved(), GameMatcher.NavigationPath.AddedOrRemoved());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasLocation;
        }

        protected override void Execute(List<GameEntity> entities)
        {

            //first, darken entire grid:
            _grid.LightPath(_grid, false);

            if (entities.Count < 1)
            {
                _game.Log("No units on route selected");
            }

            foreach (var unit in entities)
            {
                if (unit.isSelected && unit.hasNavigationPath)
                {
                    Stack<HexCellBehaviour> p = unit.navigationPath.path;
                    _grid.LightPath(p, true);
                }
            }
        }
    }
}
