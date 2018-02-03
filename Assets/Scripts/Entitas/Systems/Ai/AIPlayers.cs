using UnityEngine;
using Entitas;
using FluentBehaviourTree;
using System.Collections.Generic;
using Systems.Ai.Behaviour;

namespace Systems.Ai
{
    public class AIPlayers : IExecuteSystem
    {
        private GameContext _game;
        private IGroup<GameEntity> _aiPlayers;
        private Dictionary<int, IAIBehaviour> _aiTree;

        private IGroup<GameEntity> _bases;
        private IGroup<GameEntity> _barracks;

        public AIPlayers(Contexts contexts)
        {
            //init own variables
            _game = contexts.game;
            _aiPlayers = _game.GetGroup(GameMatcher.AIPlayer);
            _aiTree = new Dictionary<int, IAIBehaviour>();

            //init behavior tree helpers:
            _barracks = _game.GetGroup(GameMatcher.AllOf(GameMatcher.CanBuildVehicle, GameMatcher.Team));
            _bases = _game.GetGroup(GameMatcher.AllOf(GameMatcher.CanBuildBarracks, GameMatcher.Team));


            //create behavior trees:
            foreach (var player in _aiPlayers)
            {
                _aiTree[player.team.value] = new StupidBehaviour(_game, this, player);
            }
        }

        void IExecuteSystem.Execute()
        {
            foreach(var ai in _aiPlayers)
            {
                _aiTree[ai.team.value].Tick(new TimeData(Time.deltaTime));
            }
        }
        public GameEntity BarrackForTeam(int teamNr)
        {
            return ForTeam(teamNr, _barracks);
        }

        public GameEntity BaseForTeam(int teamNr)
        {
            return ForTeam(teamNr, _bases);
        }

        private GameEntity ForTeam(int teamNr, IGroup<GameEntity> grp)
        {
            foreach (var ent in grp)
            {
                if (ent.team.value == teamNr) return ent;
            }
            return null;
        }
    }
}
