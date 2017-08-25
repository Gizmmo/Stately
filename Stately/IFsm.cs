using System;

namespace Stately
{
    public interface IFsm<T> where T : IState
    {
        /// <summary>
        /// Gets the amount of states stored in the Fsm
        /// </summary>
        int StateCount { get; }

        /// <summary>
        /// Gets the type of inital state that will be used when started
        /// </summary>
        Type InitialState { get; }

        /// <summary>
        /// Returns true if the Fsm has started
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// The current state the state machine is in
        /// </summary>
        T State { get; }

        /// <summary>
        /// Adds the passed state into the state machine
        /// </summary>
        /// <param name="state">The state to put into the state machine</param>
        void AddState(IState state);

        /// <summary>
        /// Removes the passed state type from the state machine
        /// </summary>
        /// <typeparam name="TSub">The state to remove from the state machine</typeparam>
        bool RemoveState<TSub>() where TSub : T;

        /// <summary>
        /// Sets the inital state of the state machine with the type passed
        /// </summary>
        /// <typeparam name="TSub">The type to set the FSM when the machine starts</typeparam>
        void SetInitialState<TSub>() where TSub : T;

        /// <summary>
        /// Starts the FSM
        /// </summary>
        void Start();

        /// <summary>
        /// Adds a transition between 2 states
        /// </summary>
        /// <typeparam name="TStateFrom">The state that this transiton can be triggered from</typeparam>
        /// <typeparam name="TStateTo">the state the fsm will go to when the transition completes its trigger</typeparam>
        /// <param name="transition">The transition to trigger to go between these states</param>
        void AddTransition<TStateFrom, TStateTo>(ITransition transition)
            where TStateFrom : T
            where TStateTo : T;

        /// <summary>
        /// Triggers the passed transition for the Fsm's current state
        /// </summary>
        /// <typeparam name="TTransition">The transition to trigger</typeparam>
        void TriggerTransition<TTransition>() where TTransition : ITransition;

        /// <summary>
        /// Removes the passed transition from the passed state, and returns true if it was done successfully.
        /// </summary>
        /// <typeparam name="TTransition">The transition to remove</typeparam>
        /// <typeparam name="TState">The state to remove the transition from</typeparam>
        /// <returns>True if the transition was removed, false otherwise</returns>
        bool RemoveTransition<TTransition, TState>() where TTransition : ITransition
            where TState : T;
    }
}