using Entitas;
using System;
using System.Collections.Generic;

namespace Index
{

    //@TODO: make generic?
    public class IDIndex
    {

        private IGroup<GameEntity> observedCollection;
        private Dictionary<int, GameEntity> lookup = new Dictionary<int, GameEntity>();

        public IDIndex(GameContext ctx)
        {
            observedCollection = ctx.GetGroup(GameMatcher.ID);
            observedCollection.OnEntityAdded += AddEntity;
            observedCollection.OnEntityRemoved += RemoveEntity;
        }

        ~IDIndex()
        {
            Cleanup();
        }

        public void Cleanup()
        {
            observedCollection.OnEntityAdded -= AddEntity;
            observedCollection.OnEntityRemoved -= RemoveEntity;
            lookup.Clear();
        }

        public Entity FindEntityWithName(int x)
        {
            return lookup[x];
        }

        public GameEntity this[int x]
        {
            get
            {
                return lookup[x];
            }
        }


        protected virtual void AddEntity(IGroup collection, GameEntity entity, int index, IComponent component)
        {
            var nameComponent = component as IDComponent;
            if (nameComponent != null)
            {
                if (lookup.ContainsKey(nameComponent.value) && lookup[nameComponent.value] == entity)
                {
                    return;
                }

                if (lookup.ContainsKey(nameComponent.value) && lookup[nameComponent.value] != entity)
                {
                    throw new Exception("the key " + nameComponent.value + " is not unique. Present on entity: " + entity.creationIndex + " and entity: " + lookup[nameComponent.value].creationIndex);
                }
                entity.Retain(this);
                lookup[nameComponent.value] = entity;
            }

        }

        protected virtual void RemoveEntity(IGroup collection, Entity entity, int index, IComponent component)
        {
            var nameComponent = component as IDComponent;
            if (nameComponent != null && lookup.ContainsKey(nameComponent.value))
            {
                lookup[nameComponent.value].Release(this);
                lookup.Remove(nameComponent.value);
            }
        }
    }


}
