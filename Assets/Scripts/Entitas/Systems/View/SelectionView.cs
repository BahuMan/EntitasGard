using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Systems.View
{
    public class SelectionView : ReactiveSystem<GameEntity>
    {
        private SelectedContainerBehaviour _panel;

        public SelectionView(Contexts contexts): base(contexts.game)
        {
            _panel = GameObject.FindObjectOfType<SelectedContainerBehaviour>();
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Selected.AddedOrRemoved());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGameObject;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var sel in entities)
            {
                if (sel.isSelected)
                {
                    _panel.Select(sel.gameObject.value);
                }
                else
                {
                    _panel.UnSelect(sel.gameObject.value);
                }
            }
        }
    }
}
