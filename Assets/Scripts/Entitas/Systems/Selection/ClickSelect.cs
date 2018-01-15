using System.Collections.Generic;
using Entitas;
using System;
using UnityEngine;

namespace Systems.Selection
{
    public class ClickSelect : ReactiveSystem<InputEntity>
    {

        public const float MAX_CLICK_DISTANCE = 5; //nr of pixels between button up and button down

        IGroup<InputEntity> _UICommands;
        IGroup<InputEntity> _leftClicks;
        IGroup<GameEntity> _selected;
        IGroup<GameEntity> _selectable;
        InputContext _input;
        GameContext _game;

        public ClickSelect(Contexts contexts): base(contexts.input)
        {
            _input = contexts.input;
            _game = contexts.game;
            _leftClicks = _input.GetGroup(InputMatcher.MouseLeftDown);
            _UICommands = _input.GetGroup(InputMatcher.UICommand);
            _selected = _game.GetGroup(GameMatcher.Selected);
            _selectable = _game.GetGroup(GameMatcher.Selectable);
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.MouseLeftReleased);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.isMouseLeftReleased;
        }

        protected override void Execute(List<InputEntity> entities)
        {

            //if a ui command is active, this system should not interpret the mouse click as a select action
            if (_UICommands.count > 0) return;


            var cs = _leftClicks.GetEnumerator();
            cs.MoveNext();
            var clickStart = cs.Current; //there should be exactly one clickStart entity
            var clickEnd = entities[0];  //there should be exactly one clickEnd entity

            float clickPixelsWidth = Math.Abs(clickEnd.screenCoordinates.x - clickStart.screenCoordinates.x);
            float clickPixelsHeighth = Math.Abs(clickEnd.screenCoordinates.x - clickStart.screenCoordinates.x);

            if (clickPixelsHeighth < MAX_CLICK_DISTANCE && clickPixelsWidth < MAX_CLICK_DISTANCE)
            {
                PerformClickSelect(clickStart, clickEnd);
            }
            else
            {
                PerformBoxSelect(clickStart, clickEnd);
            }

        }

        //click start and end were far apart, we assume the player dragged a box
        private void PerformBoxSelect(InputEntity clickStart, InputEntity clickEnd)
        {
            float boxx = Math.Min(clickStart.screenCoordinates.x, clickEnd.screenCoordinates.x);
            float boxy = Math.Min(clickStart.screenCoordinates.y, clickEnd.screenCoordinates.y);
            float width = Math.Abs(clickStart.screenCoordinates.x - clickEnd.screenCoordinates.x);
            float height = Math.Abs(clickStart.screenCoordinates.y - clickEnd.screenCoordinates.y);

            ClearSelection();

            foreach (var unit in _selectable)
            {

                Vector3 scr = Camera.main.WorldToScreenPoint(unit.gameObject.value.transform.position);
                if (scr.x > boxx
                    && scr.x < (boxx+width)
                    && scr.y > boxy
                    && scr.y < (boxy+height))
                {
                    unit.isSelected = true;
                }
            }
        }

        //click start and end were close together, so we assumed the player clicked on a single object
        private void PerformClickSelect(InputEntity clickStart, InputEntity clickEnd)
        {
            ClearSelection();
            GameEntity ge = _game.GetEntityWithID(clickEnd.mouseOverEntity.value);
            if (ge.isSelectable)
            {
                ge.isSelected = true;
            }
            else
            {
                _game.Log("Mouse clicked on non-selectable entity " + ge.iD);
            }
        }

        private void ClearSelection()
        {
            foreach (var s in _selected.GetEntities())
            {
                s.isSelected = false;
            }
        }

    }
}
