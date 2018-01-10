using System.Collections.Generic;
using Entitas;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.View
{

    //reactive system, so this will only trigger when a debug message has been added
    public class LogDebugMessageSystem : ReactiveSystem<GameEntity>
    {
        private Text _debugText;
        System.Text.StringBuilder _builder = new System.Text.StringBuilder(500);

        public LogDebugMessageSystem(Contexts contexts) : base(contexts.game)
        {
            _debugText = GameObject.FindGameObjectWithTag("DebugText").GetComponentInChildren<Text>();
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            // we only care about entities with DebugMessageComponent 
            return context.CreateCollector(GameMatcher.DebugMessage);
        }

        protected override bool Filter(GameEntity entity)
        {
            //final check: does the entity still contain a debug message?
            return entity.hasDebugMessage;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            _builder.Length = 0;
            foreach (var entity in entities)
            {
                _builder.AppendLine(entity.debugMessage.value);
            }
            string finalString = _builder.ToString();
            //Debug.Log(finalString);
            _debugText.text = finalString;
        }

    }
}