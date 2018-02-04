namespace SimpleBehaviour
{
    public class Condition : INode
    {
        public delegate bool ConditionHandler();
        private ConditionHandler _condition;

        public Condition(ConditionHandler condition)
        {
            _condition = condition;
        }

        public TreeStatusEnum Tick()
        {
            return _condition() ? TreeStatusEnum.SUCCESS : TreeStatusEnum.FAILURE;
        }
    }
}