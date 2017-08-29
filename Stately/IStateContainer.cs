using System;

namespace Stately
{
    public interface IStateContainer<T, TTransitionsEnum> where T : IState where TTransitionsEnum : struct, IConvertible
    {
        /// <summary>
        /// The state that this container holds.
        /// </summary>
        T State { get; }

        /// <summary>
        /// The amount of transitions stored in transition Dictionary.
        /// </summary>
        int TransitionCount { get; }

        /// <summary>
        /// Gets the transition from the transition container of the transtions dictionary
        /// </summary>
        /// <param name="key">The key of the transition to get</param>
        /// <returns>The transition of the type store</returns>
        ITransitionContainer GetTransition(TTransitionsEnum key);

        void AddTransition<TStateTo>(TTransitionsEnum key, ITransition transition) where TStateTo : T;

        bool RemoveTransition(TTransitionsEnum key);

        Type TriggerTransition(TTransitionsEnum key);

        bool HasTransition<TStateTo>() where TStateTo : T;
    }
}