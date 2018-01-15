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

        public CommandView(Contexts contexts) : base(contexts.game)
        {
            _input = contexts.input;
            _UICommands = _input.GetGroup(InputMatcher.UICommand);
            _allSelected = contexts.game.GetGroup(GameMatcher.Selected);
            _panel = GameObject.FindObjectOfType<CommandPanelBehaviour>();
            _panel.AttackCommand += panel_AttackCommand;
            _panel.NavigateCommand += panel_NavigateCommand;
        }

        private void panel_NavigateCommand()
        {
            Debug.Log("Navigate UI command issues");
            foreach (var c in _UICommands.GetEntities()) c.Destroy();
            InputEntity n = _input.CreateEntity();
            n.isUICommand = true;
            n.isUINavigate = true;
        }

        private void panel_AttackCommand()
        {
            Debug.Log("Attack UI command issues");
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
            return entity.hasGameObject;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            _panel.ShowNothing();

            //ignore the entities list, and go over all currently selected units:
            foreach (var unit in _allSelected)
            {
                if (unit.hasWeapon) _panel.ShowAttack();
                if (unit.hasNavigable) _panel.ShowNavigate();
            }
        }
    }
}
