using System;
using System.Collections.Generic;

namespace ATG.Transition
{
    public sealed class TransitionService: ITransitionService
    {
        private readonly Dictionary<TransitionType, TransitionData> _dictionary;

        public TransitionService(IReadOnlyCollection<TransitionData> dataCollection)
        {
            _dictionary = new Dictionary<TransitionType, TransitionData>(dataCollection.Count);

            foreach(var e in dataCollection)
            {
                _dictionary.Add(e.Type, e);
            }
        }

        public ITransitionBehaviour<T> GetTransitionBehaviourByType<T>(TransitionType type)
        {
            if(_dictionary.ContainsKey(type) == false)
             throw new ArgumentException($"Transition with type {type} not exist !");

            return _dictionary[type].Create<T>();
        }

        public HashSet<ITransitionBehaviour<T>> GetTransitionsBehaviourByTypes<T>(params TransitionType[] types)
        {
            HashSet<ITransitionBehaviour<T>> res = new();

            foreach(var type in types)
            {
                var transition = GetTransitionBehaviourByType<T>(type);
                res.Add(transition);
            }

            return res;
        }
    }
}