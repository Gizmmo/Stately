using System;
using System.Collections.Generic;
using Stately.Exceptions;

namespace Stately
{
    public class Fsm<TConcreteState, TTransitionsEnum> : IFsm<TConcreteState, TTransitionsEnum> where TConcreteState : IState where TTransitionsEnum : struct, IConvertible
    {
        private StateContainer<TConcreteState, TTransitionsEnum> _currentStateContainer;

        private readonly Dictionary<Type, StateContainer<TConcreteState, TTransitionsEnum>> _states = new Dictionary<Type, StateContainer<TConcreteState, TTransitionsEnum>>();
        
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
        public TConcreteState CurrentState => _currentStateContainer == null ? default(TConcreteState): _currentStateContainer.State;

        /// <summary>
        /// Returns true if the Fsm has started
        /// </summary>
        public bool IsStarted { get; private set; }


        /// <summary>
        /// Adds the passed state into the state machine
        /// </summary>
        /// <param name="state">The state to put into the state machine</param>
        public void AddState<TSub>(TSub state) where TSub : TConcreteState
        {
            var key = state.GetType();

            if (_states.ContainsKey(key))
                throw new DuplicateStateException();

            _states.Add(key, new StateContainer<TConcreteState, TTransitionsEnum>(state));
        }


        /// <summary>
        /// Removes the passed state type from the state machine
        /// </summary>
        /// <typeparam name="TSub">The state to remove from the state machine</typeparam>
        public bool RemoveState<TSub>() where TSub : TConcreteState
        {
            var key = typeof (TSub);

            if (CurrentState != null && CurrentState.GetType() == key)
                throw new RemoveCurrentStateException();

            return _states.Remove(key);
        }


        /// <summary>
        /// Sets the inital state of the state machine with the type passed
        /// </summary>
        /// <typeparam name="TSub">The type to set the FSM when the machine starts</typeparam>
        public void SetInitialState<TSub>() where TSub : TConcreteState
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

        
        public void AddTransition<TStateFrom, TStateTo>(TTransitionsEnum action, ITransition transition) where TStateFrom : TConcreteState where TStateTo : TConcreteState
        {
            var foundStateFromContainer = GetStateContiainer<TStateFrom>();

            if (!IsStateFound(typeof(TStateTo)))
                StateNotFound();

            foundStateFromContainer.AddTransition<TStateTo>(action, transition);
        }
        
        public void AddTransition<TStateFrom, TStateTo>(TTransitionsEnum action, Action transition) where TStateFrom : TConcreteState where TStateTo : TConcreteState => AddTransition<TStateFrom, TStateTo>(action, new ActionTransition(transition));

        public void AddTransition<TStateFrom, TStateTo>(TTransitionsEnum action) where TStateFrom : TConcreteState where TStateTo : TConcreteState => AddTransition<TStateFrom, TStateTo>(action, new Transition());

        public void TriggerTransition(TTransitionsEnum key)
        {
            if (!IsStarted)
                throw new StateMachineNotStartedException();

            var stateTo = _currentStateContainer.TriggerTransition(key);
            SetCurrentState(stateTo);
        }
        
        public bool RemoveTransition<TState>(TTransitionsEnum action)
            where TState : TConcreteState => GetStateContiainer<TState>().RemoveTransition(action);

        /// <summary>
        /// Sets the current state to the passed state Type.
        /// </summary>
        internal void SetCurrentState<TState>() where TState : TConcreteState => SetCurrentState(typeof(TState));

        internal void SetCurrentState(Type state)
        {
            var foundStateContainer = GetStateContiainer(state);

            if (IsStarted)
                CurrentState.OnExit();

            _currentStateContainer = foundStateContainer;
            CurrentState.OnEntry();
        }
        
        /// <summary>
        /// Returns the CurrentState Container for the passed state type
        /// </summary>
        /// <param name="state">The state type to find the container of</param>
        /// <returns>The state container of the passed state</returns>
        internal StateContainer<TConcreteState, TTransitionsEnum> GetStateContiainer<TState>() where TState : TConcreteState => GetStateContiainer(typeof(TState));

        /// <summary>
        /// Returns the CurrentState Container for the passed state type
        /// </summary>
        /// <param name="state">The state type to find the container of</param>
        /// <returns>The state container of the passed state</returns>
        internal StateContainer<TConcreteState, TTransitionsEnum> GetStateContiainer(Type state)
        {
            StateContainer<TConcreteState, TTransitionsEnum> foundState;

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

        public bool HasTransition<TStateFrom, TStateTo>()
            where TStateFrom : TConcreteState
            where TStateTo : TConcreteState => GetStateContiainer<TStateFrom>().HasTransition<TStateTo>();

        public void RemoveAllTransitions<TState>() where TState : TConcreteState
        {
            GetStateContiainer<TState>().RemoveAllTransitions();
        }
    }
}