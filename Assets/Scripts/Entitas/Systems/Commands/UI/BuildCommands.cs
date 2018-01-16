using System;
using System.Collections.Generic;
using Entitas;

namespace Systems.Command.UI
{
    public class BuildCommands : ReactiveSystem<InputEntity>
    {

        private Presets _presets;
        private GameContext _game;
        private HexGridBehaviour _grid;
        private IGroup<InputEntity> _UICommands;

        public BuildCommands(Contexts contexts): base(contexts.input)
        {
            _UICommands = contexts.input.GetGroup(InputMatcher.UICommand);
            _game = contexts.game;
            _grid = UnityEngine.GameObject.FindObjectOfType<HexGridBehaviour>();
            _presets = new Presets(_game, _grid);
        }

        protected override void Execute(List<InputEntity> entities)
        {
            if (entities.Count != 1) throw new System.ArgumentException("found more than 1 left mouse clicks?");

            //this command does not apply if the necessary UI command hasn't been issued
            if (_UICommands.count == 0) return;

            if (!entities[0].hasMouseOverEntity)
            {
                foreach (var ui in _UICommands.GetEntities()) ui.Destroy();
                return;
            }

            GameEntity overEntity = _game.GetEntityWithID(entities[0].mouseOverEntity.value);

            foreach(var ui in _UICommands)
            {
                if (ui.isUIBuildBarracks) BuildBarracks(overEntity);
                if (ui.isUIBuildTower) BuildTower(overEntity);
                if (ui.isUINewVehicle) BuildNewVehicle(overEntity);
            }
        }

        private void BuildBarracks(GameEntity overEntity)
        {
            throw new NotImplementedException();
        }

        private void BuildTower(GameEntity overEntity)
        {
            throw new NotImplementedException();
        }

        private void BuildNewVehicle(GameEntity overEntity)
        {
            throw new NotImplementedException();
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasMouseOverEntity;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.MouseLeftReleased);
        }
    }
}
