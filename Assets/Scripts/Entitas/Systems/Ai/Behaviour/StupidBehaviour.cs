using UnityEngine;
using FluentBehaviourTree;
using System.Collections.Generic;

namespace Systems.Ai.Behaviour
{
    public class StupidBehaviour: IAIBehaviour
    {
        private IBehaviourTreeNode _tree;

        //variables used during behavior tree decision process:
        private GameContext _game;
        private HexGridBehaviour _grid;
        private GameEntity _player;
        private AIPlayers _allPlayers;

        public StupidBehaviour(GameContext game, AIPlayers allplayers, GameEntity playa)
        {
            _game = game;
            _player = playa;
            _allPlayers = allplayers;
            _grid = GameObject.FindObjectOfType<HexGridBehaviour>();

            var builder = new BehaviourTreeBuilder();
            builder.Sequence("BuildBarracks")
                .Inverter("not").Condition("HasBarracks", t => HasBarracks(t, playa)).End()
                .Do("BuildBarracks", t => BuildBarracks(t, playa))
                .End();

            _tree = builder.Build();
        }

        public void Tick(TimeData t)
        {
            _tree.Tick(t);
        }

        private bool HasBarracks(TimeData t, GameEntity playa)
        {
            int team = playa.team.value;
            return null != _allPlayers.BarrackForTeam(team);
        }

        private BehaviourTreeStatus BuildBarracks(TimeData t, GameEntity playa)
        {
            int team = playa.team.value;
            GameEntity home = _allPlayers.BaseForTeam(team);
            if (home == null) return BehaviourTreeStatus.Failure; //no home base => we are dead

            List<HexCellBehaviour> hood = _grid.GetWithinRange(3, home.location.cell.cubeCoordinates);

            HexCellBehaviour buildLoc = null;
            while (hood.Count > 0)
            {
                //chose random cell
                int chosen = UnityEngine.Random.Range(0, hood.Count);
                buildLoc = hood[chosen];
                int cellid = buildLoc.GetComponent<EntitasLink>().id;

                //cell is empty?
                if (_game.GetEntitiesWithLocation(cellid).Count == 0)
                {

                }
                else
                {
                    hood.RemoveAt(chosen);
                }
            }
            return BehaviourTreeStatus.Failure;
        }

    }
}
