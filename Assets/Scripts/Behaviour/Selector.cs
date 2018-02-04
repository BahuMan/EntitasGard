namespace SimpleBehaviour
{

    public class Selector : INode
    {
        int i = 0;
        INode[] _sequence;
        public Selector(params INode[] sequence)
        {
            _sequence = sequence;
        }

        public TreeStatusEnum Tick()
        {
            while (i < _sequence.Length)
            {
                switch (_sequence[i].Tick())
                {
                    case TreeStatusEnum.RUNNING:
                        return TreeStatusEnum.RUNNING; //abort and keep current index
                    case TreeStatusEnum.SUCCESS:
                        i = 0;
                        return TreeStatusEnum.SUCCESS; //abort and reset index so sequence will restart
                }
                ++i; //continue with next action
            }

            //completed the sequence; reset index and return SUCCESS
            i = 0;
            return TreeStatusEnum.FAILURE;
        }

    }
}