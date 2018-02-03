using Entitas;
using FluentBehaviourTree;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Ai
{
    public class AutoAttack : IExecuteSystem
    {
        private GameContext _game;
        private IGroup<GameEntity> _autoAttacking;
        private HexGridBehaviour _grid;
        private Dictionary<int, AutoAttackNearestBehaviour> _treeForAttacker;

        public AutoAttack(Contexts contexts)
        {
            _game = contexts.game;
            _autoAttacking = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Weapon, GameMatcher.AutoAttack));
            _grid = GameObject.FindObjectOfType<HexGridBehaviour>();

            _treeForAttacker = new Dictionary<int, AutoAttackNearestBehaviour>();
            foreach(var attacker in _autoAttacking)
            {
                _treeForAttacker[attacker.iD.value] = new AutoAttackNearestBehaviour(_game, attacker, _grid);
            }
        }

        public void Execute()
        {
            TimeData td = new TimeData(Time.deltaTime);
            foreach (var attacker in _autoAttacking)
            {
                if (!_treeForAttacker.ContainsKey(attacker.iD.value))
                {
                    _treeForAttacker[attacker.iD.value] = new AutoAttackNearestBehaviour(_game, attacker, _grid);
                }
                _treeForAttacker[attacker.iD.value].Tick(td);
            }
        }
    }
}
