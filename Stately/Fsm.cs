using System;
using System.Collections.Generic;

namespace Stately
{
    public class Fsm<T> : IFsm<T> where T : IState
    {
                /// <summary>
        /// Gets the amount of states stored in the Fsm
        /// </summary>
        public int StateCount => _states.Count;

        /// <summary>
        /// Gets the type of inital state that will be used when started
        /// </summary>
        public Type InitialState { get; private set; }

        /// <summary>
        /// The current state the state machine is in
        /// </summary>
        public T State { get; private set; }

        /// <summary>
        /// Returns true if the Fsm has started
        /// </summary>
        public bool IsStarted { get; private set; }

        private StateContainer _currentStateContainer;

        private readonly Dictionary<Type, StateContainer> _states = new Dictionary<Type, StateContainer>();


        /// <summary>
        /// Adds the passed state into the state machine
        /// </summary>
        /// <param name="state">The state to put into the state machine</param>
        public void AddState(IState state)
        {
            var key = state.GetType();

            if (_states.ContainsKey(key))
                throw new DuplicateStateException();

            state.SetUpTransition(TriggerTransition);
            _states.Add(key, new StateContainer(state));
        }


        /// <summary>
        /// Removes the passed state type from the state machine
        /// </summary>
        /// <typeparam name="TSub">The state to remove from the state machine</typeparam>
        public bool RemoveState<TSub>() where TSub : T
        {
            var key = typeof (TSub);

            if (State != null && State.GetType() == key)
                throw new RemoveCurrentStateException();

            return _states.Remove(key);
        }


        /// <summary>
        /// Sets the inital state of the state machine with the type passed
        /// </summary>
        /// <typeparam name="TSub">The type to set the FSM when the machine starts</typeparam>
        public void SetInitialState<TSub>() where TSub : T
        {
            var key = typeof (TSub);

            if (!IsStateFound(key))
                StateNotFound();

            InitialState = key;
        }


        /// <summary>
        /// Starts the FSM
        /// </summary>
        public void Start()
        {
            if (InitialState == null)
                throw new InitalStateNullException();

            SetCurrentState(InitialState);
            IsStarted = true;
        }

        /// <summary>
        /// Adds a transition between 2 states
        /// </summary>
        /// <typeparam name="TStateFrom">The state that this transiton can be triggered from</typeparam>
        /// <typeparam name="TStateTo">the state the fsm will go to when the transition completes its trigger</typeparam>
        /// <param name="transition">The transition to trigger to go between these states</param>
        public void AddTransition<TStateFrom, TStateTo>(ITransition transition)
            where TStateFrom : T
            where TStateTo : T
        {
            var foundStateFromContainer = GetStateContiainer(typeof (TStateFrom));

            var key = typeof (TStateTo);
            if (!IsStateFound(key))
                StateNotFound();

            foundStateFromContainer.AddTransition(transition, key);
        }

        /// <summary>
        /// Triggers the passed transition for the Fsm's current state
        /// </summary>
        /// <typeparam name="TTransition">The transition to trigger</typeparam>
        public void TriggerTransition<TTransition>() where TTransition : ITransition
        {
            if (!IsStarted)
                throw new StateMachineNotStartedException();

            var stateTo = _currentStateContainer.TriggerTransition<TTransition>();
            SetCurrentState(stateTo);
        }


        public void TriggerTransition(Type transition)
        {
            if (!IsStarted)
                throw new StateMachineNotStartedException();

            var stateTo = _currentStateContainer.TriggerTransition(transition);
            SetCurrentState(stateTo);
        }

        /// <summary>
        /// Removes the passed transition from the passed state, and returns true if it was done successfully.
        /// </summary>
        /// <typeparam name="TTransition">The transition to remove</typeparam>
        /// <typeparam name="TState">The state to remove the transition from</typeparam>
        /// <returns>True if the transition was removed, false otherwise</returns>
        public bool RemoveTransition<TTransition, TState>()
            where TTransition : ITransition
            where TState : T
        {
            return GetStateContiainer(typeof (TState)).RemoveTransition<TTransition>();
        }

        /// <summary>
        /// Sets the current state to the passed state Type.
        /// </summary>
        internal void SetCurrentState(Type state)
        {
            var foundStateContainer = GetStateContiainer(state);

            if (IsStarted)
                State.OnExit();

            _currentStateContainer = foundStateContainer;
            State = (T) _currentStateContainer.State;
            State.OnEntry();
        }

        /// <summary>
        /// Returns the State Container for the passed state type
        /// </summary>
        /// <param name="state">The state type to find the container of</param>
        /// <returns>The state container of the passed state</returns>
        internal StateContainer GetStateContiainer(Type state)
        {
            StateContainer foundState;

            if (!_states.TryGetValue(state, out foundState))
                StateNotFound();

            return foundState;
        }

        /// <summary>
        /// Returns true if the state is in the Fsm, false otherwise
        /// </summary>
        /// <param name="key">The state name</param>
        /// <returns>true if the state is in the Fsm, false otherwise</returns>
        internal bool IsStateFound(Type key)
        {
            return _states.ContainsKey(key);
        }

        /// <summary>
        /// Throws a StateNotFoundException
        /// </summary>
        internal void StateNotFound()
        {
            throw new StateNotFoundException();
        }
    }
    
    /// <summary>
    /// Thrown when an inital state is not set for start up.
    /// </summary>
    public class InitalStateNullException : Exception
    {
        public InitalStateNullException()
        {
        }

        public InitalStateNullException(string message)
            : base(message)
        {
        }

        public InitalStateNullException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Thrown when a state does not exist is trying to be accessed.
    /// </summary>
    public class StateNotFoundException : Exception
    {
        public StateNotFoundException()
        {
        }

        public StateNotFoundException(string message)
            : base(message)
        {
        }

        public StateNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Thrown when a state is being removed that is set as the current state.
    /// </summary>
    public class RemoveCurrentStateException : Exception
    {
        public RemoveCurrentStateException()
        {
        }

        public RemoveCurrentStateException(string message)
            : base(message)
        {
        }

        public RemoveCurrentStateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class DuplicateStateException : Exception
    {
        public DuplicateStateException()
        {
        }

        public DuplicateStateException(string message)
            : base(message)
        {
        }

        public DuplicateStateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class DuplicateStateTransitionException : Exception
    {
        public DuplicateStateTransitionException()
        {
        }

        public DuplicateStateTransitionException(string message)
            : base(message)
        {
        }

        public DuplicateStateTransitionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class TransitionNotFoundException : Exception
    {
        public TransitionNotFoundException()
        {
        }

        public TransitionNotFoundException(string message)
            : base(message)
        {
        }

        public TransitionNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class StateMachineNotStartedException : Exception
    {
        public StateMachineNotStartedException()
        {
        }

        public StateMachineNotStartedException(string message)
            : base(message)
        {
        }

        public StateMachineNotStartedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class InvalidTransitionTypeException : Exception
    {
        public InvalidTransitionTypeException()
        {
        }

        public InvalidTransitionTypeException(string message)
            : base(message)
        {
        }

        public InvalidTransitionTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}