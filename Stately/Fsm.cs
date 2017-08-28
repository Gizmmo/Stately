using System;
using System.Collections.Generic;
using Stately.Exceptions;

namespace Stately
{
    public class Fsm<T> : IFsm<T> where T : IState
    {
        private StateContainer _currentStateContainer;

        private readonly Dictionary<Type, StateContainer> _states = new Dictionary<Type, StateContainer>();
        
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
            where TState : T => GetStateContiainer(typeof (TState)).RemoveTransition<TTransition>();

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
        internal bool IsStateFound(Type key) => _states.ContainsKey(key);

        /// <summary>
        /// Throws a StateNotFoundException
        /// </summary>
        internal void StateNotFound() => throw new StateNotFoundException();
    }
}