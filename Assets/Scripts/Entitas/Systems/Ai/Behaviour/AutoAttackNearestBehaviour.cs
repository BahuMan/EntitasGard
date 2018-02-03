using UnityEngine;
using Entitas;
using FluentBehaviourTree;
using System;
using System.Collections.Generic;

public class AutoAttackNearestBehaviour
{
    private GameContext _game;
    private GameEntity _turret;
    private HexGridBehaviour _grid;
    private IBehaviourTreeNode _tree;
    private int[] _CellIdInRange;
    private List<HexCellBehaviour> inRange;
    private GameEntity closestEnemy = null;

    //private const float IDLE_DELAY = 2f;
    //private float _currentIdleDelayed = 0f;
    private const float ATTACK_DELAY = 5f;
    private float _currentAttackDelayed = 0f;

    public AutoAttackNearestBehaviour(GameContext game, GameEntity turret, HexGridBehaviour grid)
    {
        _game = game;
        _turret = turret;
        _grid = grid;

        inRange = _grid.GetWithinRange(_turret.weapon.range, _turret.location.cell.cubeCoordinates);
        _CellIdInRange = new int[inRange.Count];
        //for(int i=0; i<_CellIdInRange.Length; ++i)
        //{
        //    _CellIdInRange[i] = inRange[i].GetComponent<EntitasLink>().id;
        //}

        _tree = new BehaviourTreeBuilder()
            .Sequence("turret main loop")
                .Selector("find target")
                    .Do("keep existing target", KeepExistingTarget)
                    .Do("Find new Target", FindClosestEnemy)
                .End()
                .Do("AttackTarget", AttackTarget)
            .End()
            .Build();
    }

    private BehaviourTreeStatus KeepExistingTarget(TimeData t)
    {
        if (!_turret.hasAttackTarget) return BehaviourTreeStatus.Failure;
        if (!_turret.isAutoAttacking) return BehaviourTreeStatus.Success;

        GameEntity target = _game.GetEntityWithID(_turret.attackTarget.targetID);

        //target still alive?
        if (target == null) return BehaviourTreeStatus.Failure;
        if (target.health.value < 0) return BehaviourTreeStatus.Failure;
        if (target.isKilled) return BehaviourTreeStatus.Failure;

        //out of range?
        int distance = _grid.DistanceBetween(_turret.location.cell, target.location.cell);
        if (distance > _turret.weapon.range) return BehaviourTreeStatus.Failure;

        return BehaviourTreeStatus.Success;
    }

    private BehaviourTreeStatus FindClosestEnemy(TimeData t)
    {
        if (_turret.hasAttackTarget && _turret.isAutoAttacking) _turret.RemoveAttackTarget();

        closestEnemy = null;
        int closestDistance = int.MaxValue;
        foreach(var cellid in _CellIdInRange)
        {
            HashSet<GameEntity> candidates = _game.GetEntitiesWithLocation(cellid);
            foreach(var candidate in candidates)
            {
                if (!IsEnemy(candidate)) continue;
                int dist = _grid.DistanceBetween(candidate.location.cell, _turret.location.cell);
                if (dist < closestDistance)
                {
                    closestEnemy = candidate;
                    closestDistance = dist;
                }
            }
        }

        return closestEnemy == null ? BehaviourTreeStatus.Failure : BehaviourTreeStatus.Success;
    }

    private bool IsEnemy(GameEntity candidate)
    {
        return _turret.team.value != candidate.team.value;
    }

    private BehaviourTreeStatus AttackTarget(TimeData t)
    {
        if (closestEnemy == null) return BehaviourTreeStatus.Failure;

        //were we here before? If so, only count the time
        if (_currentAttackDelayed != 0f)
        {
            _currentAttackDelayed += t.deltaTime;
            if (_currentAttackDelayed > ATTACK_DELAY)
            {
                _currentAttackDelayed = 0;
                return BehaviourTreeStatus.Success;
            }
            else
            {
                return BehaviourTreeStatus.Running;
            }
        }
        _turret.ReplaceAttackTarget(closestEnemy.iD.value);
        _turret.isAutoAttacking = true;
        _currentAttackDelayed = float.Epsilon;
        return BehaviourTreeStatus.Running;
    }

    public void Tick(TimeData t)
    {
        _tree.Tick(t);
    }
}
