using UnityEngine;
using Entitas;
using System;

namespace Systems.Input
{
    public class SelectionShortcuts : IExecuteSystem
    {

        private IGroup<GameEntity> _selectableBase;
        private IGroup<GameEntity> _selectableUnits;
        private IGroup<GameEntity> _allSelected;

        public SelectionShortcuts(Contexts contexts)
        {
            _selectableBase = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.CanBuildBarracks, GameMatcher.UnderControl, GameMatcher.Selectable));
            _selectableUnits = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Weapon, GameMatcher.Navigable, GameMatcher.Selectable));
            _allSelected = contexts.game.GetGroup(GameMatcher.Selected);
        }

        public void Execute()
        {
            if (UnityEngine.Input.GetButtonDown("SelectHome")) SelectHome();
            if (UnityEngine.Input.GetButtonDown("SelectAllUnits")
                && UnityEngine.Input.GetButton("SelectAllUnitsModifier")) SelectAllUnits();
        }

        private void SelectAllUnits()
        {
            Select(_allSelected, false);
            Select(_selectableUnits, true);
        }

        private void SelectHome()
        {
            Select(_allSelected, false);
            Select(_selectableBase, true);
        }

        private void Select(IGroup<GameEntity> list, bool selected)
        {
            foreach(var sel in list.GetEntities())
            {
                sel.isSelected = selected;
            }
        }
    }
}
