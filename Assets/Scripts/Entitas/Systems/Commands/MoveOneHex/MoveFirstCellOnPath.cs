using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Systems.Command.MoveOneHex
{
    public class MoveFirstCellOnPath : IExecuteSystem
    {

        public float CLOSE_ENOUGH = 0.001f;
        GameContext _game;
        IGroup<GameEntity> _navigatingUnits;

        public MoveFirstCellOnPath(Contexts contexts)
        {
            _game = contexts.game;
            _navigatingUnits = _game.GetGroup(GameMatcher.AllOf(
                GameMatcher.WorldCoordinates,
                GameMatcher.Location,
                GameMatcher.NavigationPath,
                GameMatcher.Navigable));
        }

        public void Execute()
        {
            foreach (var unit in _navigatingUnits)
            {
                HexCellBehaviour curCell = unit.location.cell;
                Stack<HexCellBehaviour> path = unit.navigationPath.path;

                //three possibilities:
                //1. we need to rotate before moving towards the next cell
                //2. we're moving to the next cell but still in our "own" cell
                //3. we're already in the next cell but still moving to the center
                //4. we've arrived (and this is possibly the last move)

                //the code checks above in the opposite order:

                float todist = CalcSqrDistance(unit.worldCoordinates, path.Peek());

                if (todist < CLOSE_ENOUGH)
                {
                    //4. we've arrived
                    path.Pop();
                    if (path.Count > 0) unit.ReplaceNavigationPath(path);
                    else unit.RemoveNavigationTarget();
                    _game.Log("ARRIVED");
                    break;
                }

                float fromdist = CalcSqrDistance(unit.worldCoordinates, curCell);
                if (todist < fromdist)
                {
                    //3. we're closer to next cell but still moving to center
                    unit.ReplaceLocation(path.Peek(), path.Peek().GetComponent<EntitasLink>().id);
                    //no break, we need to add movement vector
                }


                Vector3 dir = unit.navigable.moveRate * Time.deltaTime * CalcVector3(unit.worldCoordinates, path.Peek());
                float roty = CalcRotationY(unit.worldCoordinates, dir, unit.navigable.turnRate);

                if (Mathf.Abs(roty) < CLOSE_ENOUGH)
                {
                    //2.we're pointed at next cell but still in our own
                    unit.ReplaceMove(dir.x, dir.z, roty);
                    _game.Log("moving");
                    break;
                }

                //1. not oriented towards destination cell; unit should only rotate and not move
                unit.ReplaceMove(0, 0, roty);
                _game.Log("rotating");

            }
        }

        private float CalcRotationY(WorldCoordinatesComponent worldCoordinates, Vector3 dir, float rotationSpeed)
        {
            Quaternion from = new Quaternion(worldCoordinates.rx, worldCoordinates.ry, worldCoordinates.rz, worldCoordinates.rw);
            Quaternion to = Quaternion.LookRotation(dir, Vector3.up);
            return Quaternion.RotateTowards(from, to, rotationSpeed * Time.deltaTime).eulerAngles.y;
        }

        private float CalcSqrDistance(WorldCoordinatesComponent coord, HexCellBehaviour to)
        {
            return (new Vector3(coord.x, coord.y, coord.z) - to.transform.position).sqrMagnitude;
        }

        private Vector3 CalcVector3(WorldCoordinatesComponent coord, HexCellBehaviour to)
        {
            return new Vector3(
                to.transform.position.x - coord.x,
                to.transform.position.y - coord.y,
                to.transform.position.z - coord.z
                ).normalized;
        }
    }
}
