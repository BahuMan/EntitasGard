using UnityEngine;
using System.Collections.Generic;
using SimpleBehaviour;

namespace Systems.Ai.Behaviour
{
    public class StupidBehaviour: IAIBehaviour
    {
        private INode _tree;

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

            _tree = new Sequence(
                new Inverter(new Condition(() => HasBarracks(playa))),
                new Action(() => BuildBarracks(playa))
                );

        }

        public void Tick()
        {
            _tree.Tick();
        }

        private bool HasBarracks(GameEntity playa)
        {
            int team = playa.team.value;
            return null != _allPlayers.BarrackForTeam(team);
        }

        private TreeStatusEnum BuildBarracks(GameEntity playa)
        {
            int team = playa.team.value;
            GameEntity home = _allPlayers.BaseForTeam(team);
            if (home == null) return TreeStatusEnum.FAILURE; //no home base => we are dead

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
            return TreeStatusEnum.FAILURE;
        }

    }
}
