using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Systems.View
{
    public class MoveInUnity : ReactiveSystem<GameEntity>
    {

        public MoveInUnity(Contexts contexts) : base(contexts.game)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Move);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasMove && entity.hasGameObject;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                Transform t = e.gameObject.value.transform;

                t.position += new Vector3(e.move.dx, 0, e.move.dz);
                t.rotation *= Quaternion.Euler(0, e.move.ry, 0);

                e.ReplaceWorldCoordinates(t.position.x, t.position.y, t.position.z,
                                            t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w);
            }
        }
    }
}
