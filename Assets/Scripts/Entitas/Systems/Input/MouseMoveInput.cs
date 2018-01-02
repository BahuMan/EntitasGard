using System;
using Entitas;
using UnityEngine;

namespace Systems.Input
{
    public class MouseMoveInput : IExecuteSystem
    {
        InputContext _input;

        public MouseMoveInput(Contexts contexts)
        {
            _input = contexts.input;
        }

        void IExecuteSystem.Execute()
        {
            if (UnityEngine.Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                RaycastHit hitinfo;
                if (Physics.Raycast(ray, out hitinfo))
                {
                    HexCellBehaviour navTarget = hitinfo.collider.GetComponentInParent<HexCellBehaviour>();
                    Debug.Log("right mouse button found " + navTarget.name);
                }
            }
        }
    }
}
