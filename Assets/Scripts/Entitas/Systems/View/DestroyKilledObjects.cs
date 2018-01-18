using UnityEngine;
using Entitas;
using System.Collections.Generic;

namespace Systems.View
{
    public class DestroyKilledObjects : ReactiveSystem<GameEntity>
    {
        public DestroyKilledObjects(Contexts contexts) : base(contexts.game)
        {
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var todestroy in entities)
            {
                GameObject.Destroy(todestroy.gameObject.value);
                todestroy.RemoveGameObject();
            }
        }

        protected override bool Filter(GameEntity entity)
        {
            return (entity.hasGameObject && entity.isKilled);
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.GameObject, GameMatcher.Killed));
        }
    }
}
