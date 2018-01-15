using System.Collections.Generic;
using Entitas;

namespace Systems.Command.Attack
{
    public class UILeftClickAttack : ReactiveSystem<InputEntity>
    {

        private GameContext _game;
        private IGroup<GameEntity> _selectedAttackingUnits;
        private IGroup<InputEntity> _UICommands;

        public UILeftClickAttack(Contexts contexts) : base(contexts.input)
        {
            _game = contexts.game;
            _selectedAttackingUnits = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected, GameMatcher.Weapon));
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
            //quit when another (non-attack) UI command is active
            foreach (var ui in _UICommands.GetEntities())
            {
                if (!ui.isUIAttack) return; //leave alone and abort
                else ui.Destroy();          //click removes attack command, whether successful or not
            }

            if (entities.Count != 1) throw new System.ArgumentException("found " + entities.Count + " left mouse click?");

            InputEntity click = entities[0];
            GameEntity targetEntity = _game.GetEntityWithID(click.mouseOverEntity.value);
            DefaultRightClickAttackCommand.AttackTarget(_game, targetEntity, _selectedAttackingUnits);

        }
    }
}