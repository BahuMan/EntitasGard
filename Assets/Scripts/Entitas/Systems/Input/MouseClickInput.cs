using System;
using Entitas;
using UnityEngine;
using Index;
using UnityEngine.UI;

namespace Systems.Input
{
    public class MouseClickInput : IExecuteSystem, ICleanupSystem
    {

        private Text DebugText;

        //we create and clean entities in input context,
        IGroup<InputEntity> _mouseInputs;

        InputContext _input;
        public MouseClickInput(Contexts contexts)
        {

            DebugText = GameObject.FindGameObjectWithTag("DebugText").GetComponentInChildren<Text>();

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

                //any gameobject also in entitas should have a component "EntitasLink" in its hierarchy:
                EntitasLink el = hitinfo.collider.gameObject.GetComponentInParent<EntitasLink>();

                if (el != null)
                {
                    DebugText.text = "Found id in unity component: " + el.id;
                    m.AddMouseOverEntity(el.id);
                }
                else
                {
                    Debug.LogError("Object with a collider does not have an Entitas link component: " + hitinfo.collider.name);
                }
            }
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var e in _mouseInputs.GetEntities())
            {
                e.Destroy();
            }
        }

    }
}
