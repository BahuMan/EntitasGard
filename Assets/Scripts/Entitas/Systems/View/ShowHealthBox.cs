using UnityEngine;
using Entitas;
using System.Collections.Generic;

namespace Systems.View
{
    public class ShowHealthBox : ReactiveSystem<GameEntity>
    {
        private RectTransform _OSD;
        private HealthBoxBehaviour _prefab;

        public ShowHealthBox(Contexts contexts) : base(contexts.game)
        {
            _OSD = GameObject.FindGameObjectWithTag("OnScreenDisplay").GetComponent<RectTransform>();
            _prefab = Resources.Load<HealthBoxBehaviour>("Prefabs/HealthBox");
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                if (e.isKilled || !e.hasHealth)
                {
                    GameObject.Destroy(e.unityHealthBox.value.gameObject);
                    e.RemoveUnityHealthBox();
                }
                else if (e.hasHealth)
                {
                    if (!e.hasUnityHealthBox) CreateHealthBox(e);
                    e.unityHealthBox.value.Health = e.health.value;
                }
            }
        }

        private void CreateHealthBox(GameEntity e)
        {
            HealthBoxBehaviour hb = GameObject.Instantiate(_prefab);
            hb.SetTarget(e.gameObject.value);
            hb.GetComponent<RectTransform>().SetParent(_OSD);
            hb.MaxHealth = e.health.value;
            e.AddUnityHealthBox(hb);
        }

        protected override bool Filter(GameEntity entity)
        {
            return true;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Health.AddedOrRemoved(), GameMatcher.Killed.Added());
        }
    }
}
