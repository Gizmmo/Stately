using System;
using System.Collections.Generic;
using System.Linq;
using Stately.Exceptions;

namespace Stately
{
    public class StateContainer<T, TTransitionsEnum> : IStateContainer<T, TTransitionsEnum> where T : IState where TTransitionsEnum : struct, IConvertible
    {
        private readonly Dictionary<TTransitionsEnum, ITransitionContainer> _transitions = new Dictionary<TTransitionsEnum, ITransitionContainer>();

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="state">The state this container will store</param>
        public StateContainer(T state)
        {
            State = state;
        }

        /// <summary>
        /// The state that this container holds.
        /// </summary>
        public T State { get; }

        /// <summary>
        /// The amount of transitions stored in transition Dictionary.
        /// </summary>
        public int TransitionCount => _transitions.Count;

        public void AddTransition<TStateTo>(TTransitionsEnum key, ITransition transition) where TStateTo : T
        {
            if (_transitions.ContainsKey(key))
                throw new DuplicateStateTransitionException();

            _transitions.Add(key , new TransitionContainer(transition, typeof(TStateTo)));
        }

        public ITransitionContainer GetTransition(TTransitionsEnum transition)
        {
            ITransitionContainer foundTransition;

            if (!_transitions.TryGetValue(transition, out foundTransition))
                throw new TransitionNotFoundException();

            return foundTransition;
        }

        public bool RemoveTransition(TTransitionsEnum key)
        {
            return _transitions.Remove(key);
        }

        public Type TriggerTransition(TTransitionsEnum transition)
        {
            var container = GetTransition(transition);
            container.Transition.Trigger();
            return container.StateTo;
        }

        public bool HasTransition<TStateTo>() where TStateTo : T => _transitions.Any(transition => transition.Value.StateTo == typeof(TStateTo));
    }
}