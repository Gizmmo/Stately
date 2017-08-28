using System;

namespace Stately
{
    public interface IState
    {
        /// <summary>
        /// Called On Entry into the state
        /// </summary>
        void OnEntry();

        /// <summary>
        /// Called on Exit from the state 
        /// </summary>
        void OnExit();

    }
}