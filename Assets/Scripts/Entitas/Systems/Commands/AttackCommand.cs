using System.Collections.Generic;
using Entitas;
using System;

namespace Systems.Command
{
    public class AttackCommand : ReactiveSystem<InputEntity>
    {
        private GameContext _game;
        private IGroup<GameEntity> _selectedAttackingUnits;

        public AttackCommand(Contexts contexts): base(contexts.input)
        {
            _game = contexts.game;
            _selectedAttackingUnits = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected, GameMatcher.Weapon));
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
            if (entities.Count != 1) throw new ArgumentException("found " + entities.Count + " right mouse click?");

            InputEntity click = entities[0];
            GameEntity attackTarget = _game.GetEntityWithID(click.mouseOverEntity.value);

            if (!attackTarget.hasHealth)
            {
                //maybe we clicked the location rather than the unit to be attacked?
                if (attackTarget.hasHexCell && attackTarget.hasID)
                {
                    //I expect only one object per location, but I'm not checking:
                    foreach (var t in _game.GetEntitiesWithLocation(attackTarget.iD.value))
                    {
                        if (t.hasHealth) attackTarget = t;
                    }
                }
            }

            //again, check (because the right-click might have been a navigation
            //command and in that case we need to fail silently
            if (attackTarget.hasHealth)
            {
                foreach (var unit in _selectedAttackingUnits)
                {
                    unit.ReplaceAttackTarget(attackTarget.iD.value);
                }
            }
            else
            {
                //for debugging purposes, we fail loudly :)
                _game.Log("right click was not an attack command");
                foreach (var unit in _selectedAttackingUnits)
                {
                    if (unit.hasAttackTarget) unit.RemoveAttackTarget();
                }

            }
        }
    }
}
