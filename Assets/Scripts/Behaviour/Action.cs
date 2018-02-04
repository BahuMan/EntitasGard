namespace SimpleBehaviour
{
    public class Action : INode
    {
        public delegate TreeStatusEnum TickHandler();

        private TickHandler _tick;
        public Action(TickHandler t)
        {
            _tick = t;
        }

        public TreeStatusEnum Tick()
        {
            return _tick();
        }
    }
}