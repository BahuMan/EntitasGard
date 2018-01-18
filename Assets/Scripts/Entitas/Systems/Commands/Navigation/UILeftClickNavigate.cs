using Entitas;
using System.Collections.Generic;

namespace Systems.Command.Navigation
{
    public class UILeftClickNavigate : ReactiveSystem<InputEntity>
    {
        private GameContext _game;
        private IGroup<GameEntity> _selectedNavigableUnits;
        private IGroup<InputEntity> _UICommands;

        public UILeftClickNavigate(Contexts contexts) : base(contexts.input)
        {
            _game = contexts.game;
            _selectedNavigableUnits = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected, GameMatcher.Navigable, GameMatcher.UnderControl));
            _UICommands = contexts.input.GetGroup(InputMatcher.UICommand);
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

            //this system does not apply when no UI command active
            if (_UICommands.count == 0) return;

            //quit when another (non-navigate) UI command is active
            foreach (var ui in _UICommands.GetEntities())
            {
                if (!ui.isUINavigate) return; //leave alone and abort
                else ui.Destroy();           //click removes attack command, whether successful or not
            }

            if (entities.Count != 1) throw new System.ArgumentException("found " + entities.Count + " left mouse click?");

            InputEntity click = entities[0];
            GameEntity dest = _game.GetEntityWithID(click.mouseOverEntity.value);

            if (!dest.hasHexCell && dest.hasLocation)
            {
                dest = _game.GetEntityWithID(dest.location.cellid);
            }
            foreach (var unit in _selectedNavigableUnits)
            {
                if (unit.iD.value == dest.iD.value) continue; //don't move to self
                if (unit.location.cellid == dest.iD.value) continue; //don't move to current location
                    unit.ReplaceNavigationTarget(dest.iD.value);
            }

        }
    }
}
