using System.Collections.Generic;

namespace ATG.Transition
{
    public interface ITransitionService
    {
        ITransitionBehaviour<T> GetTransitionBehaviourByType<T>(TransitionType type);
        HashSet<ITransitionBehaviour<T>> GetTransitionsBehaviourByTypes<T>(params TransitionType[] types);
    }
}