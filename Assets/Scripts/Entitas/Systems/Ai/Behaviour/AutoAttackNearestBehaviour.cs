using UnityEngine;
using System.Collections.Generic;
using SimpleBehaviour;

public class AutoAttackNearestBehaviour
{
    private GameContext _game;
    private GameEntity _turret;
    private HexGridBehaviour _grid;
    private INode _tree;
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
        for (int i = 0; i < _CellIdInRange.Length; ++i)
        {
            _CellIdInRange[i] = inRange[i].GetComponent<EntitasLink>().id;
        }

        _tree =
            new Sequence(
                new Selector(
                    new Action(KeepExistingTarget),
                    new Action(FindClosestEnemy)
                    ),
                new Action(AttackTarget)
                );
    }

    private TreeStatusEnum KeepExistingTarget()
    {
        if (!_turret.hasAttackTarget) return TreeStatusEnum.FAILURE;
        if (!_turret.isAutoAttacking) return TreeStatusEnum.SUCCESS;

        GameEntity target = _game.GetEntityWithID(_turret.attackTarget.targetID);

        //target still alive?
        if (target == null) return TreeStatusEnum.FAILURE;
        if (target.health.value < 0) return TreeStatusEnum.FAILURE;
        if (target.isKilled) return TreeStatusEnum.FAILURE;

        //out of range?
        int distance = _grid.DistanceBetween(_turret.location.cell, target.location.cell);
        if (distance > _turret.weapon.range) return TreeStatusEnum.FAILURE;

        return TreeStatusEnum.SUCCESS;
    }

    private TreeStatusEnum FindClosestEnemy()
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

        return closestEnemy == null ? TreeStatusEnum.FAILURE : TreeStatusEnum.SUCCESS;
    }

    private bool IsEnemy(GameEntity candidate)
    {
        return _turret.team.value != candidate.team.value;
    }

    private TreeStatusEnum AttackTarget()
    {
        if (closestEnemy == null) return TreeStatusEnum.FAILURE;

        //were we here before? If so, only count the time
        if (_currentAttackDelayed != 0f)
        {
            _currentAttackDelayed += Time.deltaTime;
            if (_currentAttackDelayed > ATTACK_DELAY)
            {
                _currentAttackDelayed = 0;
                return TreeStatusEnum.SUCCESS;
            }
            else
            {
                return TreeStatusEnum.RUNNING;
            }
        }
        _turret.ReplaceAttackTarget(closestEnemy.iD.value);
        _turret.isAutoAttacking = true;
        _currentAttackDelayed = float.Epsilon;
        return TreeStatusEnum.RUNNING;
    }

    public void Tick()
    {
        _tree.Tick();
    }
}
