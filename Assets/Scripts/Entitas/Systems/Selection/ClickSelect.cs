using System.Collections.Generic;
using Entitas;
using System;

namespace Systems.Selection
{
    public class ClickSelect : ReactiveSystem<InputEntity>
    {

        private const float _maxClickTime = .5f; //time between button down and button up
        private const float _maxClickDistance = 10; //nr of pixels between button up and button down

        IGroup<InputEntity> _leftClicks;
        IGroup<GameEntity> _selected;
        InputContext _input;
        GameContext _game;

        public ClickSelect(Contexts contexts): base(contexts.input)
        {
            _input = contexts.input;
            _game = contexts.game;
            _leftClicks = _input.GetGroup(InputMatcher.MouseLeftDown);
            _selected = _game.GetGroup(GameMatcher.Selected);
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
            var cs = _leftClicks.GetEnumerator();
            cs.MoveNext();
            var clickStart = cs.Current; //there should be exactly one clickStart entity
            var clickEnd = entities[0];  //there should be exactly one clickEnd entity

            float clickTime = clickEnd.timeLine.value - clickStart.timeLine.value;
            float clickPixelsWidth = Math.Abs(clickEnd.screenCoordinates.x - clickStart.screenCoordinates.x);
            float clickPixelsHeighth = Math.Abs(clickEnd.screenCoordinates.x - clickStart.screenCoordinates.x);

            if (clickTime < _maxClickTime 
                || (clickPixelsHeighth < _maxClickDistance && clickPixelsWidth < _maxClickDistance))
            {
                GameEntity ge = _game.GetEntityWithID(clickEnd.mouseOverEntity.value);
                if (ge.isSelectable)
                {
                    ClearSelection();
                    ge.isSelected = true;
                }
                else
                {
                    _game.Log("Mouse clicked on non-selectable entity " + ge.iD);
                }
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
