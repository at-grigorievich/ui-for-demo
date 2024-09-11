namespace ATG.Transition
{
    public interface ITransitionData
    {
        public TransitionType Type {get;}
        ITransitionBehaviour<T> Create<T>();
    }
}