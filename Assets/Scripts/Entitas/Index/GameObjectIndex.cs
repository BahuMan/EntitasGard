using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Index
{
    public class GameObjectIndex
    {
        private IGroup<GameEntity> observedCollection;
        private Dictionary<GameObject, GameEntity> lookup = new Dictionary<GameObject, GameEntity>();

        public GameObjectIndex(GameContext ctx)
        {
            observedCollection = ctx.GetGroup(GameMatcher.GameObject);
            observedCollection.OnEntityAdded += AddEntity;
            observedCollection.OnEntityRemoved += RemoveEntity;
        }

        ~GameObjectIndex()
        {
            Cleanup();
        }

        public void Cleanup()
        {
            observedCollection.OnEntityAdded -= AddEntity;
            observedCollection.OnEntityRemoved -= RemoveEntity;
            lookup.Clear();
        }

        public Entity FindEntityForGameObject(GameObject go)
        {
            return lookup[go];
        }

        public GameEntity this[GameObject go]
        {
            get
            {
                return lookup[go];
            }
        }

        public bool ContainsGameObject(GameObject go)
        {
            return lookup.ContainsKey(go);
        }

        public bool ContainsEntity(GameEntity ge)
        {
            return lookup.ContainsValue(ge);
        }

        public IEnumerable<GameObject> AllGameObjects()
        {
            return lookup.Keys;
        }

        public IEnumerable<GameEntity> AllEntities()
        {
            return lookup.Values;
        }

        public int Count
        {
            get
            {
                return lookup.Count;
            }
        }

        protected virtual void AddEntity(IGroup collection, GameEntity entity, int index, IComponent component)
        {
            var nameComponent = component as GameObjectComponent;
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
            var nameComponent = component as GameObjectComponent;
            if (nameComponent != null && lookup.ContainsKey(nameComponent.value))
            {
                lookup[nameComponent.value].Release(this);
                lookup.Remove(nameComponent.value);
            }
        }
    }
}
