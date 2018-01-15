using System.Collections.Generic;
using Entitas;

namespace Systems.Command.Navigation
{
    public class NavigationCommand : ReactiveSystem<InputEntity>
    {

        private GameContext _game;

        private IGroup<GameEntity> _selectedNavigableUnits;

        public NavigationCommand(Contexts contexts): base(contexts.input)
        {
            _game = contexts.game;
            _selectedNavigableUnits = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Navigable, GameMatcher.Selected));
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.MouseRightDown);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.isMouseRightDown && entity.hasMouseOverEntity;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            if (entities.Count != 1) throw new System.ArgumentException("too many right click entities");

            InputEntity rightClick = entities[0];
            GameEntity dest = _game.GetEntityWithID(rightClick.mouseOverEntity.value);
            if (!dest.hasHexCell && dest.hasLocation)
            {
                dest = _game.GetEntityWithID(dest.location.cellid);
            }
            foreach (var unit in _selectedNavigableUnits)
            {
                unit.ReplaceNavigationTarget(dest.iD.value);
            }
        }
    }
}
