using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Transitions
{
    /// <summary>
    /// The available transition kinds
    /// </summary>
    public enum TransitionKind
    {
        FadeOut,
        FadeIn
    }

    class TransitionFactory
    {
        private static Dictionary<TransitionKind, BaseTransition> transitions;

        /// <summary>
        /// Get an transition based on it's kind. 
        /// It will new the transition if it hasn't been used yet. 
        /// This will also call the Reset from BaseTransition
        /// </summary>
        /// <param name="kind">The kind of transition</param>
        /// <returns>An resetted transition based on the kind</returns>
        public static BaseTransition GetTransition(TransitionKind kind)
        {
            if(transitions == null)
            {
                transitions = new Dictionary<TransitionKind, BaseTransition>();
            }

            if (!transitions.Keys.Contains<TransitionKind>(kind))
            {
                switch(kind)
                {
                    case TransitionKind.FadeIn:
                        transitions.Add(kind, new FadeInTransition());
                        break;
                    case TransitionKind.FadeOut:
                        transitions.Add(kind, new FadeOutTransition());
                        break;
                }
            }


            BaseTransition transition = transitions[kind];
            transition.Reset();
            return transition;
        }
    }
}
