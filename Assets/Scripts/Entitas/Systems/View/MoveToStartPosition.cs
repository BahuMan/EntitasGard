using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Systems.View
{
    public class MoveToStartPosition : ReactiveSystem<GameEntity>
    {
        public MoveToStartPosition(Contexts contexts): base(contexts.game)
        {
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                Transform t = e.gameObject.value.transform;
                Vector3 pos = new Vector3(e.startPosition.x, e.startPosition.y, e.startPosition.z);
                t.position = pos;
                e.ReplaceWorldCoordinates(pos.x, pos.y, pos.z, t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.z);
            }
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGameObject && entity.hasStartPosition;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.StartPosition);
        }
    }
}
