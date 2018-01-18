using System.Collections.Generic;
using Entitas;

namespace Systems.Command
{
    public class UnderControlByLocalPlayers : ReactiveSystem<GameEntity>
    {
        IGroup<GameEntity> _localPlayers;

        public UnderControlByLocalPlayers(Contexts contexts): base(contexts.game)
        {
            _localPlayers = contexts.game.GetGroup(GameMatcher.LocalPlayer);
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach(var unit in entities)
            {
                if (unit.hasTeam && IsLocalTeam(unit.team.value))
                {
                    unit.isUnderControl = true;
                }
                else
                {
                    unit.isUnderControl = false;
                }
            }
        }

        private bool IsLocalTeam(int t)
        {
            foreach (var player in _localPlayers)
                if (player.team.value == t)
                    return true;
            return false;
        }

        protected override bool Filter(GameEntity entity)
        {
            return true;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Team.AddedOrRemoved());
        }
    }
}
