using System;
using Entitas;
using UnityEngine;

namespace Systems.Input
{
    public class MouseSelectInput : IExecuteSystem
    {
        private InputContext _input;
        private GameContext _game;

        private IGroup<InputEntity> _selectInputs;
        private IGroup<GameEntity> _selectedUnits;
        private IGroup<GameEntity> _selectableUnits;

        public MouseSelectInput(Contexts contexts)
        {
            _input = contexts.input;
            _game = contexts.game;

            _selectInputs = _input.GetGroup(InputMatcher.SelectionBox);
            _selectedUnits = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected));
            _selectableUnits = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Selectable));
        }

        void IExecuteSystem.Execute()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                InputEntity startSelect = _input.CreateEntity();
                startSelect.AddSelectionBox(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, 0, 0);

                if (!UnityEngine.Input.GetButton("AdditiveSelection"))
                {
                    ClearSelection();
                }
            }
            else if (UnityEngine.Input.GetMouseButton(0))
            {
                InputEntity box = _selectInputs.GetSingleEntity();
                float newwidth = box.selectionBox.x - UnityEngine.Input.mousePosition.x;
                float newheight = box.selectionBox.y - UnityEngine.Input.mousePosition.y;
                box.ReplaceSelectionBox(box.selectionBox.x, box.selectionBox.y, newwidth, newheight);
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                InputEntity box = _selectInputs.GetSingleEntity();

                //selection rectangle:
                float minx = Mathf.Min(box.selectionBox.x, UnityEngine.Input.mousePosition.x);
                float maxx = Mathf.Max(box.selectionBox.x, UnityEngine.Input.mousePosition.x);
                float miny = Mathf.Min(box.selectionBox.y, UnityEngine.Input.mousePosition.y);
                float maxy = Mathf.Max(box.selectionBox.y, UnityEngine.Input.mousePosition.y);

                Debug.Log("Checking " + _selectableUnits.count + " selectable units");
                foreach (var unit in _selectableUnits)
                {
                    Vector3 screencoordinates = UnityEngine.Camera.main.WorldToScreenPoint(unit.gameObject.value.transform.position);
                    if (minx < screencoordinates.x && screencoordinates.x < maxx
                        && miny < screencoordinates.y && screencoordinates.y < maxy)
                    {
                        unit.isSelected = true;
                    } 
                }

                box.Destroy();
            }
        }

        private void ClearSelection()
        {
            foreach(var unit in _selectedUnits.GetEntities())
            {
                unit.isSelected = false;
            }
        }
    }
}
