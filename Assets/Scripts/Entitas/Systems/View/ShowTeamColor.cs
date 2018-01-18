using UnityEngine;
using Entitas;
using System.Collections.Generic;

namespace Systems.View
{
    public class ShowTeamColor : ReactiveSystem<GameEntity>
    {

        private Color[] teamcolor;

        public ShowTeamColor(Contexts contexts): base(contexts.game)
        {
            Object[] res = Resources.LoadAll("PlayerColors/");
            teamcolor = new Color[res.Length];
            for (int i=0; i<res.Length; ++i)
            {
                teamcolor[i] = ((Material)res[i]).color;
            }
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                TeamColor tc = entity.gameObject.value.GetComponent<TeamColor>();
                if (tc != null)
                {
                    tc.teamNr = entity.team.value;
                    tc.teamColor = teamcolor[entity.team.value];
                }
            }
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasTeam && entity.hasGameObject;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Team);
        }
    }
}

