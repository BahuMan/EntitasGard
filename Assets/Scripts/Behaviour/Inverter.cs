namespace SimpleBehaviour
{
    public class Inverter : INode
    {
        private INode _node;

        public Inverter(INode n)
        {
            _node = n;
        }

        public TreeStatusEnum Tick()
        {
            switch (_node.Tick())
            {
                case TreeStatusEnum.FAILURE: return TreeStatusEnum.SUCCESS;
                case TreeStatusEnum.SUCCESS: return TreeStatusEnum.FAILURE;
                default: return TreeStatusEnum.RUNNING;
            }
        }
    }

}
