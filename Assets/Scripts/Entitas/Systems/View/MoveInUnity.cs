using System.Collections.Generic;
using Entitas;

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
                e.gameObject.value.transform.Translate(e.move.dx, 0, e.move.dz);
                e.gameObject.value.transform.Rotate(0, e.move.ry, 0);
            }
        }
    }
}
