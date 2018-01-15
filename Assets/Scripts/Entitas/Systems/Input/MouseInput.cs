using Entitas;
using UnityEngine;

namespace Systems.Input
{
    public class MouseInput : IExecuteSystem, ICleanupSystem
    {

        //we create and clean entities in input context,
        IGroup<InputEntity> _leftClicks; //can contain at most one entity
        IGroup<InputEntity> _rightClicks; //can contain at most one entity
        IGroup<InputEntity> _mouseHover;

        InputContext _input;
        public MouseInput(Contexts contexts)
        {
            _input = contexts.input;
            _leftClicks = _input.GetGroup(InputMatcher.AnyOf(InputMatcher.MouseLeftDown, InputMatcher.MouseLeftReleased));
            _rightClicks = _input.GetGroup(InputMatcher.AnyOf(InputMatcher.MouseRightDown, InputMatcher.MouseRightReleased));
            _mouseHover = _input.GetGroup(InputMatcher.MouseHover);
        }


        void IExecuteSystem.Execute()
        {
            //ignore interaction with UI:
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            InputEntity m = _input.CreateEntity();

            //always add screen coordinates:
            m.AddScreenCoordinates((int)UnityEngine.Input.mousePosition.x, (int)UnityEngine.Input.mousePosition.y);
            //always add time this was created:
            m.AddTimeLine(Time.time);

            //did we click, or just hover?
            //@TODO: do I still need the "hover" component?
            m.isMouseHover = true; //default is hover; this will be set to false in any other case

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                RemoveAll(_leftClicks); //previous left click is no longer valid
                m.isMouseLeftDown = true;
                m.isMouseHover = false;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                m.isMouseLeftReleased = true;
                m.isMouseHover = false;
            }
            else if (_leftClicks.count > 0 && !UnityEngine.Input.GetMouseButton(0))
            {
                //we seem to have missed the button "up" event; happens e.g. during debug
                RemoveAll(_leftClicks);
            }

            if (UnityEngine.Input.GetMouseButtonDown(1))
            {
                RemoveAll(_rightClicks); //previous right click is no longer valid
                m.isMouseRightDown = true;
                m.isMouseHover = false;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(1))
            {
                m.isMouseRightReleased = true;
                m.isMouseHover = false;
            }
            else if (_rightClicks.count > 0 && !UnityEngine.Input.GetMouseButton(1))
            {
                //we seem to have missed the button "up" event; happens e.g. during debug
                RemoveAll(_rightClicks);
            }


            //now find out what the mouse is pointing at:
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo))
            {
                //any gameobject also in entitas should have a component "EntitasLink" in its hierarchy:
                EntitasLink el = hitinfo.collider.gameObject.GetComponentInParent<EntitasLink>();

                if (el != null)
                {
                    m.AddMouseOverEntity(el.id);
                }
                else
                {
                    Debug.LogError("Object with a collider does not have an Entitas link component: " + hitinfo.collider.name);
                }
            }

        }

        private void RemoveAll(IGroup<InputEntity> g)
        {
            foreach (var e in g.GetEntities())
            {
                e.Destroy();
            }
        }

        //automatic cleanup only cleans up the mouse hover
        void ICleanupSystem.Cleanup()
        {
            RemoveAll(_mouseHover);

            if (_leftClicks.count > 1) RemoveAll(_leftClicks); //2 entities means mouse has been pressed AND released
            if (_rightClicks.count > 1) RemoveAll(_rightClicks); //2 entities means mouse has been pressed AND released
        }
    }
}
