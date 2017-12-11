using System.Collections.Generic;
using Entitas;
using UnityEngine;

//reactive system, so this will only trigger when a debug message has been added
public class LogDebugMessageSystem : ReactiveSystem<GameEntity> {

    public LogDebugMessageSystem(Contexts contexts): base(contexts.game)
    {

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
        foreach (var entity in entities)
        {
            Debug.Log(entity.debugMessage.value);
        }
    }



}
