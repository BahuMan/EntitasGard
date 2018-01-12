using UnityEngine;
using Entitas;
using System.Collections.Generic;

namespace Systems.View.Attack
{

    /**
     * This system should be added AFTER MoveInUnity
     * In order to provide correct feedback about current rotation of turret
     */
    public class RotateTurretInUnity : ReactiveSystem<GameEntity>
    {
        public RotateTurretInUnity(Contexts contexts): base(contexts.game)
        {

        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            //not only when turret rotates, but also when unit moves, we need to feed back current turret rotation
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.RotateWeapon, GameMatcher.Move));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGameObject && (entity.hasRotateWeapon || entity.hasMove);
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var unit in entities)
            {
                foreach (Transform child in unit.gameObject.value.transform)
                {
                    if (!"Turret".Equals(child.name)) continue;

                    if (unit.hasRotateWeapon)
                    {
                        child.localRotation *= Quaternion.Euler(0, unit.rotateWeapon.ry, 0);
                        unit.RemoveRotateWeapon();
                    }
                    //feedback to ECS:
                    unit.ReplaceWeaponRotation(child.rotation.eulerAngles.y);
                }
            }
        }

    }
}
