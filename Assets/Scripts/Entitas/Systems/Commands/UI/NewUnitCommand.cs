using Entitas;
using System.Collections.Generic;

namespace Systems.Command.UI
{
    public class NewUnitCommand: ReactiveSystem<InputEntity>
    {
        private Presets _presets;
        private GameContext _game;
        private HexGridBehaviour _grid;
        private IGroup<GameEntity> _selectedBarracks;

        public NewUnitCommand(Contexts contexts): base(contexts.input)
        {
            _game = contexts.game;
            _grid = UnityEngine.GameObject.FindObjectOfType<HexGridBehaviour>();
            _presets = new Presets(_game, _grid);
            _selectedBarracks = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected, GameMatcher.CanBuildVehicle));
        }

        protected override void Execute(List<InputEntity> entities)
        {
            if (entities.Count != 1) throw new System.ArgumentException("found more than 1 UI build command?");

            foreach(var barracks in _selectedBarracks)
            {
                GameEntity newUnit = _presets.CreateBlueprint(Presets.EntitasPresetEnum.VEHICLE);
                newUnit.ReplaceLocation(barracks.location.cell, barracks.location.cellid);
                var cellpos = barracks.location.cell.transform.position;
                newUnit.ReplaceStartPosition(cellpos.x, cellpos.y, cellpos.z);
                newUnit.ReplaceTeam(barracks.team.value);
            }

            //whether or not command was succesful, ui command can be removed
            entities[0].Destroy();
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.isUINewVehicle;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.UINewVehicle);
        }
    }
}
