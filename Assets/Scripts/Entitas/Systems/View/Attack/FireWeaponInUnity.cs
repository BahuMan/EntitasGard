using UnityEngine;
using Entitas;
using System.Collections.Generic;

namespace Systems.View.Attack
{
    public class FireWeaponInUnity : ReactiveSystem<GameEntity>
    {
        public FireWeaponInUnity(Contexts contexts) : base(contexts.game)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.FireWeapon);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGameObject && entity.hasFireWeapon;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var unit in entities)
            {
                Debug.Log("Fire!");
            }
        }
    }
}
