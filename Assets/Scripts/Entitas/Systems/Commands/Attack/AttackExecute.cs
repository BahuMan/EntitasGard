using Entitas;
using UnityEngine;

namespace Systems.Command.Attack
{
    public class AttackExecute : IExecuteSystem
    {
        private HexGridBehaviour _grid;
        private GameContext _game;
        private IGroup<GameEntity> _attackingUnits;

        public AttackExecute(Contexts contexts)
        {
            _game = contexts.game;
            _attackingUnits = _game.GetGroup(GameMatcher.AllOf(GameMatcher.WeaponAimed, GameMatcher.AttackTarget));
            _grid = GameObject.FindObjectOfType<HexGridBehaviour>();
        }

        void IExecuteSystem.Execute()
        {
            foreach (var unit in _attackingUnits.GetEntities())
            {
                GameEntity attackTarget = _game.GetEntityWithID(unit.attackTarget.targetID);
                if (attackTarget == null)
                {
                    unit.RemoveAttackTarget();
                    continue; //no more attacking; continue with next unit
                }

                //no friendly fire
                if (unit.team.value == attackTarget.team.value)
                {
                    unit.RemoveAttackTarget();
                    continue;
                }

                int nrCells = _grid.DistanceBetween(unit.location.cell, attackTarget.location.cell);
                //out of range; don't shoot
                if (nrCells > unit.weapon.range) continue;

                //not aimed; don't shoot
                //if (!unit.isWeaponAimed) continue; // => this is enforced using GameMatcher

                //still reloading; don't shoot
                if (unit.hasFireWeapon && (unit.fireWeapon.time + 1 / unit.weapon.rateOfFire) > Time.time) continue;

                //we got here, we're ready to shoot
                unit.ReplaceFireWeapon(Time.time);

            }
        }
    }
}
