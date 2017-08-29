using System;

namespace Stately
{
    public interface IFsm<TConcreteState, TTransitionsEnum> where TConcreteState : IState where TTransitionsEnum : struct, IConvertible
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
        TConcreteState CurrentState { get; }

        /// <summary>
        /// Adds the passed state into the state machine
        /// </summary>
        /// <param name="state">The state to put into the state machine</param>
        void AddState<TSub>(TSub state) where TSub : TConcreteState;

        /// <summary>
        /// Removes the passed state type from the state machine
        /// </summary>
        /// <typeparam name="TSub">The state to remove from the state machine</typeparam>
        bool RemoveState<TSub>() where TSub : TConcreteState;

        /// <summary>
        /// Sets the inital state of the state machine with the type passed
        /// </summary>
        /// <typeparam name="TSub">The type to set the FSM when the machine starts</typeparam>
        void SetInitialState<TSub>() where TSub : TConcreteState;

        /// <summary>
        /// Starts the FSM
        /// </summary>
        void Start();

        void AddTransition<TStateFrom, TStateTo>(TTransitionsEnum action, ITransition transition)
            where TStateFrom : TConcreteState
            where TStateTo : TConcreteState;

        void AddTransition<TStateFrom, TStateTo>(TTransitionsEnum action, Action transition)
            where TStateFrom : TConcreteState where TStateTo : TConcreteState;


        void TriggerTransition(TTransitionsEnum action);

        bool RemoveTransition<TState>(TTransitionsEnum action) where TState : TConcreteState;
    }
}