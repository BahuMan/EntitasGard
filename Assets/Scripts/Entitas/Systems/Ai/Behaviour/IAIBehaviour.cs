using FluentBehaviourTree;

namespace Systems.Ai.Behaviour
{
    public interface IAIBehaviour
    {
        void Tick(TimeData t);
    }
}
