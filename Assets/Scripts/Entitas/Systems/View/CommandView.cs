using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Systems.View
{
    public class CommandView : ReactiveSystem<GameEntity>
    {
        private CommandPanelBehaviour _panel;
        private IGroup<GameEntity> _allSelected;
        private InputContext _input;
        private IGroup<InputEntity> _UICommands;
        private IGroup<GameEntity> _localPlayers;

        public CommandView(Contexts contexts) : base(contexts.game)
        {
            _input = contexts.input;
            _UICommands = _input.GetGroup(InputMatcher.UICommand);
            _allSelected = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected, GameMatcher.UnderControl));
            _panel = GameObject.FindObjectOfType<CommandPanelBehaviour>();
            _panel.AttackCommand += panel_AttackCommand;
            _panel.NavigateCommand += panel_NavigateCommand;
            _panel.BuildBarracksCommand += panel_BuildBarracksCommand;
            _panel.BuildTowerCommand += panel_BuildTowerCommand;
            _panel.newVehicleCommand += panel_newVehicleCommand;
        }

        private void panel_newVehicleCommand()
        {
            foreach (var c in _UICommands.GetEntities()) c.Destroy();
            InputEntity n = _input.CreateEntity();
            n.isUICommand = true;
            n.isUINewVehicle = true;
        }

        private void panel_BuildTowerCommand()
        {
            foreach (var c in _UICommands.GetEntities()) c.Destroy();
            InputEntity n = _input.CreateEntity();
            n.isUICommand = true;
            n.isUIBuildTower = true;
        }

        private void panel_BuildBarracksCommand()
        {
            foreach (var c in _UICommands.GetEntities()) c.Destroy();
            InputEntity n = _input.CreateEntity();
            n.isUICommand = true;
            n.isUIBuildBarracks = true;
        }

        private void panel_NavigateCommand()
        {
            foreach (var c in _UICommands.GetEntities()) c.Destroy();
            InputEntity n = _input.CreateEntity();
            n.isUICommand = true;
            n.isUINavigate = true;
        }

        private void panel_AttackCommand()
        {
            foreach (var c in _UICommands.GetEntities()) c.Destroy();
            InputEntity n = _input.CreateEntity();
            n.isUICommand = true;
            n.isUIAttack = true;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Selected.AddedOrRemoved());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGameObject; // && entity.isUnderControl;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            _panel.ShowNothing();

            //ignore the entities list, and go over all currently selected units:
            foreach (var unit in _allSelected)
            {
                if (unit.hasWeapon) _panel.ShowAttack();
                if (unit.hasNavigable) _panel.ShowNavigate();
                if (unit.isCanBuildBarracks) _panel.ShowBuildBarracks();
                if (unit.isCanBuildTower) _panel.ShowBuildTower();
                if (unit.isCanBuildVehicle) _panel.ShowNewVehicle();
            }
        }
    }
}
