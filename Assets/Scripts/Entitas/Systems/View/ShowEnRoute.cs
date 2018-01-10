using UnityEngine;
using Entitas;
using System.Collections.Generic;

namespace Systems.View
{
    public class ShowEnRoute : IExecuteSystem
    {

        IGroup<GameEntity> _unitsEnRouteSelected;
        GameContext _game;
        HexGridBehaviour _grid;

        public ShowEnRoute(Contexts contexts)
        {
            _game = contexts.game;
            _unitsEnRouteSelected = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected, GameMatcher.NavigationCommand));
            _grid = GameObject.FindObjectOfType<HexGridBehaviour>();
        }

        void IExecuteSystem.Execute()
        {
            //first, darken entire grid:
            _grid.LightPath(_grid, false);

            if (_unitsEnRouteSelected.count < 1)
            {
                _game.Log("No units on route selected");
            }

            foreach (var unit in _unitsEnRouteSelected)
            {
                HexCellBehaviour fromCell = unit.location.cell;

                GameEntity toCellEntity = _game.GetEntityWithID(unit.navigationCommand.targetCellID);
                HexCellBehaviour toCell = toCellEntity.gameObject.value.GetComponent<HexCellBehaviour>();

                Stack<HexCellBehaviour> p = _grid.FindPath(fromCell, toCell);
                _grid.LightPath(p, true);
            }
        }
    }
}
