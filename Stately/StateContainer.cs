using System;
using System.Collections.Generic;
using System.Linq;
using Stately.Exceptions;

namespace Stately
{
    public class StateContainer<T> : IStateContainer<T> where T : IState
    {
        private readonly Dictionary<Type, ITransitionContainer> _transitions = new Dictionary<Type, ITransitionContainer>();

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

        /// <summary>
        /// Adds a transition to the transition dictionary, which will be stored in a transition container, with also the state that
        /// it will go to upon completion.
        /// </summary>
        /// <param name="transition">The transition to store in the transition Dictionary</param>
        /// <param name="stateTo">The state the fsm will change to upon completion of the transition.</param>
        public void AddTransition(ITransition transition, Type stateTo)
        {
            var key = transition.GetType();

            if (_transitions.ContainsKey(key))
                throw new DuplicateStateTransitionException();

            _transitions.Add(transition.GetType(), new TransitionContainer(transition, stateTo));
        }

        /// <summary>
        /// Gets the transition from the transition container of the transtions dictionary
        /// </summary>
        /// <typeparam name="TTransition">The trpe of transition to get</typeparam>
        /// <returns>The transition of the type store</returns>
        public ITransitionContainer GetTransition<TTransition>() where TTransition : ITransition
        {
            return GetTransition(typeof (TTransition));
        }

        /// <summary>
        /// Gets the transition from the transition container of the transtions dictionary
        /// </summary>
        /// <param name="transition">The trpe of transition to get</param>
        /// <returns>The transition of the type store</returns>
        public ITransitionContainer GetTransition(Type transition)
        {
            CheckIsTransitionType(transition);
            ITransitionContainer foundTransition;

            if (!_transitions.TryGetValue(transition, out foundTransition))
                throw new TransitionNotFoundException();

            return foundTransition;
        }

        /// <summary>
        /// Removes a transition for the transitions array
        /// </summary>
        /// <typeparam name="TTransition"></typeparam>
        /// <returns>true if the transition was removed, otherwise false.</returns>
        public bool RemoveTransition<TTransition>() where TTransition : ITransition
        {
            return _transitions.Remove(typeof (TTransition));
        }

        /// <summary>
        /// Triggers the Transition searched for, and then return the state the fsm should go to.
        /// </summary>
        /// <typeparam name="TTransition">The transition to trigger.</typeparam>
        /// <returns>The state the fsm should switch to in System.Type</returns>
        public Type TriggerTransition<TTransition>() where TTransition : ITransition
        {
            return TriggerTransition(typeof (TTransition));
        }


        /// <summary>
        /// Triggers the Transition searched for, and then return the state the fsm should go to.
        /// </summary>
        /// <param name="transition">The transition to trigger</param>
        /// <returns>The state the fsm should switch to in System.Type</returns>
        public Type TriggerTransition(Type transition)
        {
            CheckIsTransitionType(transition);
            var container = GetTransition(transition);
            container.Transition.Trigger();
            return container.StateTo;
        }

        /// <summary>
        /// Checks to see if the passed type is a subclass of the ITransition class.  If not, it throws a InvalidTransitionTypeException.
        /// </summary>
        /// <param name="transition">The transition Type to check is a subclass of ITransition</param>
        private static void CheckIsTransitionType(Type transition)
        {            
            //If the passed type is not a subclass of ITransition, throw a InvalidTransitionTypeException
            if (!typeof (ITransition).IsAssignableFrom(transition))
                throw new InvalidTransitionTypeException(transition.ToString());
        }

        public bool HasTransition<TStateTo>() where TStateTo : T => _transitions.Any(transition => transition.Value.StateTo == typeof(TStateTo));
    }
}