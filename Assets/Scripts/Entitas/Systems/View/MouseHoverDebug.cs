using System.Collections.Generic;
using Entitas;

namespace Systems.View
{
    public class MouseHoverDebug : ReactiveSystem<InputEntity>
    {

        GameContext _game;

        public MouseHoverDebug(Contexts contexts) : base(contexts.input)
        {
            _game = contexts.game;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.AnyOf(InputMatcher.MouseLeftDown, InputMatcher.MouseLeftReleased));
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasMouseOverEntity;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var mo in entities)
            {
                GameEntity log = _game.CreateEntity();
                log.AddDebugMessage("" + entities.Count + " mouse hovers over entity id " + mo.mouseOverEntity.value);
            }
        }
    }
}
