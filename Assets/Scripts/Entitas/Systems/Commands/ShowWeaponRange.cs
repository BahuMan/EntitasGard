using UnityEngine;
using Entitas;

namespace Systems.Command
{
    public class ShowWeaponRange : IExecuteSystem
    {
        private IGroup<GameEntity> _selectedUnitsWithWeapons;
        private HexGridBehaviour _grid;

        public ShowWeaponRange(Contexts contexts)
        {
            _selectedUnitsWithWeapons = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected, GameMatcher.Weapon, GameMatcher.Location));
            _grid = GameObject.FindObjectOfType<HexGridBehaviour>();
        }
        public void Execute()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                foreach (var unit in _selectedUnitsWithWeapons)
                {
                    foreach (var cell in _grid.GetWithinRange(unit.weapon.range, unit.location.cell.cubeCoordinates))
                    {
                        cell.SetHighLight(true);
                    }
                }
            }
            else if (UnityEngine.Input.GetKeyUp(KeyCode.R))
            {
                foreach (var unit in _selectedUnitsWithWeapons)
                {
                    foreach (var cell in _grid.GetWithinRange(unit.weapon.range, unit.location.cell.cubeCoordinates))
                    {
                        cell.SetHighLight(false);
                    }
                }
            }
        }
    }
}
