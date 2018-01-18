using System.Collections.Generic;
using Entitas;

namespace Systems.Logic
{
    public class WeaponApplyDamage : ReactiveSystem<GameEntity>
    {
        private GameContext _game;

        public WeaponApplyDamage(Contexts contexts) : base(contexts.game)
        {
            _game = contexts.game;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var attacker in entities)
            {
                GameEntity defender = _game.GetEntityWithID(attacker.attackTarget.targetID);
                defender.ReplaceDamage(attacker.weapon.dmg, attacker.iD.value);
            }
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasWeapon && entity.hasFireWeapon && entity.hasAttackTarget;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.FireWeapon);
        }
    }
}
