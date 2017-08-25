using System;

namespace Stately
{
    public interface IStateContainer
    {
        /// <summary>
        /// The state that this container holds.
        /// </summary>
        IState State { get; }

        /// <summary>
        /// The amount of transitions stored in transition Dictionary.
        /// </summary>
        int TransitionCount { get; }

        /// <summary>
        /// Gets the transition from the transition container of the transtions dictionary
        /// </summary>
        /// <typeparam name="T">The trpe of transition to get</typeparam>
        /// <returns>The transition of the type store</returns>
        ITransitionContainer GetTransition<T>() where T : ITransition;

        /// <summary>
        /// Gets the transition from the transition container of the transtions dictionary
        /// </summary>
        /// <param name="transition">The trpe of transition to get</param>
        /// <returns>The transition of the type store</returns>
        ITransitionContainer GetTransition(Type transition);


        /// <summary>
        /// Adds a transition to the transition dictionary, which will be stored in a transition container, with also the state that
        /// it will go to upon completion.
        /// </summary>
        /// <param name="transition">The transition to store in the transition Dictionary</param>
        /// <param name="stateTo">The state the fsm will change to upon completion of the transition.</param>
        void AddTransition(ITransition transition, Type stateTo);

        /// <summary>
        /// Removes a transition for the transitions array
        /// </summary>
        /// <returns>true if the transition was removed, otherwise false.</returns>
        bool RemoveTransition<TTransition>() where TTransition : ITransition;

        /// <summary>
        /// Triggers the Transition searched for, and then return the state the fsm should go to.
        /// </summary>
        /// <typeparam name="T">The transition to trigger.</typeparam>
        /// <returns></returns>
        Type TriggerTransition<T>() where T : ITransition;

        /// <summary>
        /// Triggers the Transition searched for, and then return the state the fsm should go to.
        /// </summary>
        /// <param name="transition">The transition to trigger</param>
        /// <returns>The state the fsm should switch to in System.Type</returns>
        Type TriggerTransition(Type transition);
    }
}