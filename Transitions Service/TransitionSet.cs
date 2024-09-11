using System;
using System.Collections.Generic;

namespace ATG.Transition
{
    public sealed class TransitionSet<T>: IDisposable
    {
        private readonly HashSet<ITransitionBehaviour<T>> _transitions;

        public TransitionSet(ITransitionService transitionService, params TransitionType[] types)
        {
            _transitions = transitionService.GetTransitionsBehaviourByTypes<T>(types);
        }
        
        public void Execute(T slave)
        {
            foreach(var transition in _transitions)
            {
                transition.Execute(slave);
            }
        }

        public void Dispose()
        {
            foreach(var transition in _transitions)
            {
                transition.Dispose();
            }
        }
    }
}