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
            _UICommands = contexts.input.GetGroup(InputMatcher.AnyOf(InputMatcher.UIBuildBarracks, InputMatcher.UIBuildTower));
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
            if (!overEntity.hasHexCell) overEntity = _game.GetEntityWithID(overEntity.location.cellid);
            if (!overEntity.hasHexCell) return;

            foreach (var ui in _UICommands.GetEntities())
            {
                if (ui.isUIBuildBarracks) BuildBarracks(overEntity);
                if (ui.isUIBuildTower) BuildTower(overEntity);
                if (ui.isUINewVehicle) BuildNewVehicle(overEntity);
                ui.Destroy();
            }
        }

        private void BuildBarracks(GameEntity location)
        {
            GameEntity b = _presets.CreateBlueprint(Presets.EntitasPresetEnum.BARRACKS);
            b.AddStartPosition(location.hexCell.worldx, location.hexCell.worldy, location.hexCell.worldz);
            b.ReplaceLocation(location.gameObject.value.GetComponent<HexCellBehaviour>(), location.iD.value);
        }

        private void BuildTower(GameEntity location)
        {
            GameEntity b = _presets.CreateBlueprint(Presets.EntitasPresetEnum.TURRET);
            b.AddStartPosition(location.hexCell.worldx, location.hexCell.worldy, location.hexCell.worldz);
            b.ReplaceLocation(location.gameObject.value.GetComponent<HexCellBehaviour>(), location.iD.value);
        }

        private void BuildNewVehicle(GameEntity location)
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
