using System;
using Entitas;
using UnityEngine;
using Index;

namespace Systems.Input
{
    public class MouseClickInput : IExecuteSystem, ICleanupSystem
    {

        //we create and clean entities in input context,
        IGroup<InputEntity> _mouseInputs;

        //but mouse hovers over objects from game context:
        GameObjectIndex _gameObjectIndex;

        InputContext _input;
        public MouseClickInput(Contexts contexts)
        {
            _gameObjectIndex = new GameObjectIndex(contexts.game);
            _input = contexts.input;
            _mouseInputs = _input.GetGroup(InputMatcher.AnyOf(InputMatcher.MouseHover, InputMatcher.MouseLeftClick, InputMatcher.MouseRightClick, InputMatcher.MouseOverEntity));
        }


        void IExecuteSystem.Execute()
        {

            //now find out what the mouse is pointing at:
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo))
            {
                InputEntity m = _input.CreateEntity();

                //did we click, or just hover?
                if (UnityEngine.Input.GetMouseButtonDown(0)) m.isMouseLeftClick = true;
                else if (UnityEngine.Input.GetMouseButtonDown(1)) m.isMouseRightClick = true;
                else m.isMouseHover = true;
                
                if (_gameObjectIndex.ContainsGameObject(hitinfo.collider.gameObject))
                {
                    GameEntity e = _gameObjectIndex[hitinfo.collider.gameObject];
                    m.AddMouseOverEntity(e.iD.value);
                }
                else
                {
                    HexCellBehaviour cell = hitinfo.collider.GetComponentInParent<HexCellBehaviour>();

                    if (cell != null)
                    {
                        if (_gameObjectIndex.ContainsGameObject(cell.gameObject))
                        {
                            m.AddMouseOverEntity(_gameObjectIndex[cell.gameObject].iD.value);
                        }
                        else
                        {
                            Debug.LogError("could not find cell in index: " + cell.name + ", index size = " + _gameObjectIndex.Count);
                        }
                    }
                    else
                    {
                        Debug.LogError("mouse hovering over unknown object " + hitinfo.collider.name);
                    }
                }
            }
        }

        void ICleanupSystem.Cleanup()
        {
            //foreach (var e in _mouseInputs.GetEntities())
            //{
            //    e.Destroy();
            //}
        }

    }
}
