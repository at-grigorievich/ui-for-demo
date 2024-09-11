using System;

namespace ATG.Transition
{
    public interface ITransitionBehaviour<T>: IDisposable
    {
        void Execute(T slave, Action callback = null);
    }
}