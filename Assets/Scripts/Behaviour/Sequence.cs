namespace SimpleBehaviour
{
    public class Sequence : INode
    {
        int i = 0;
        INode[] _sequence;
        public Sequence(params INode[] sequence)
        {
            _sequence = sequence;
        }

        public TreeStatusEnum Tick()
        {
            while (i<_sequence.Length)
            {
                switch (_sequence[i].Tick())
                {
                    case TreeStatusEnum.RUNNING:
                        return TreeStatusEnum.RUNNING; //abort and keep current index
                    case TreeStatusEnum.FAILURE:
                        i = 0;
                        return TreeStatusEnum.FAILURE; //abort and reset index so sequence will restart
                }
                ++i; //continue with next action
            }

            //completed the sequence; reset index and return SUCCESS
            i = 0;
            return TreeStatusEnum.SUCCESS;
        }
    }
}