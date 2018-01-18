using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Systems.View
{
    public class SelectionView : ReactiveSystem<GameEntity>
    {
        private SelectedContainerBehaviour _panel;

        private GameContext _game;
        private IGroup<GameEntity> _allSelected;

        public SelectionView(Contexts contexts): base(contexts.game)
        {
            _game = contexts.game;
            _allSelected = _game.GetGroup(GameMatcher.Selected);
            _panel = GameObject.FindObjectOfType<SelectedContainerBehaviour>();
            _panel.SelectRequested += panel_SelectRequested;
        }

        private void panel_SelectRequested(GameObject go)
        {
            EntitasLink el = go.GetComponent<EntitasLink>();
            if (el != null) {
                ClearSelection();
                GameEntity toSelect = _game.GetEntityWithID(el.id);
                if (toSelect.isSelectable) toSelect.isSelected = true;
            }
            else
            {
                Debug.LogError("Object was passed from the UI Selection Panel that was not a selectable Entity: " + go.name);
            }

        }

        private void ClearSelection()
        {
            foreach (var s in _allSelected.GetEntities())
            {
                s.isSelected = false;
            }
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
                    if (sel.hasUnityHealthBox) sel.unityHealthBox.value.ShowFrame = true;
                }
                else
                {
                    _panel.UnSelect(sel.gameObject.value);
                    if (sel.hasUnityHealthBox) sel.unityHealthBox.value.ShowFrame = false;
                }
            }
        }
    }
}
