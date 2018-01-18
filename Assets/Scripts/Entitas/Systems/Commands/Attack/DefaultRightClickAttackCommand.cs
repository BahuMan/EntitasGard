using System.Collections.Generic;
using Entitas;
using System;

namespace Systems.Command.Attack
{
    public class DefaultRightClickAttackCommand : ReactiveSystem<InputEntity>
    {
        private GameContext _game;
        private IGroup<GameEntity> _selectedAttackingUnits;

        public DefaultRightClickAttackCommand(Contexts contexts): base(contexts.input)
        {
            _game = contexts.game;
            _selectedAttackingUnits = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected, GameMatcher.Weapon, GameMatcher.UnderControl));
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
            GameEntity targetEntity = _game.GetEntityWithID(click.mouseOverEntity.value);
            AttackTarget(_game, targetEntity, _selectedAttackingUnits);
        }

        //made static, so I can call the function in UILeftClickAttack
        public static void AttackTarget(GameContext _game, GameEntity targetEntity, IGroup<GameEntity> _selectedAttackingUnits)
        {
            if (!targetEntity.hasHealth)
            {
                //maybe we clicked the location rather than the unit to be attacked?
                if (targetEntity.hasHexCell && targetEntity.hasID)
                {
                    //I expect only one object per location, but I'm not checking:
                    foreach (var t in _game.GetEntitiesWithLocation(targetEntity.iD.value))
                    {
                        if (t.hasHealth) targetEntity = t;
                    }
                }
            }

            //again, check (because the right-click might have been a navigation
            //command and in that case we need to fail silently
            if (targetEntity.hasHealth)
            {
                foreach (var unit in _selectedAttackingUnits)
                {
                    if (unit.iD.value == targetEntity.iD.value) continue; //don't attack self
                    unit.ReplaceAttackTarget(targetEntity.iD.value);
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
