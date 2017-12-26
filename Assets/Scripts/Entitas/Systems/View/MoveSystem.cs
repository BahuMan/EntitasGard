using UnityEngine;
using Entitas;
using System;

namespace Systems.View
{
    public class MoveSystem : IExecuteSystem, ICleanupSystem
    {

        IGroup<GameEntity> _movingStuff;

        public MoveSystem(Contexts contexts)
        {
            _movingStuff = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Move, GameMatcher.ComponentsGameObject));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var mov in _movingStuff)
            {
                Transform toMove = mov.componentsGameObject.value.transform;
                Rigidbody rigid = mov.componentsGameObject.value.GetComponent<Rigidbody>();

                rigid.velocity = new Vector3(mov.move.dx, 0, mov.move.dz);
                rigid.angularVelocity = new Vector3(0, mov.move.ry, 0);
            }
        }

        void ICleanupSystem.Cleanup()
        {
            throw new NotImplementedException();
        }
    }
}
