using UnityEngine;
using Contexts;

namespace Characters
{
    public abstract class State
    {
        protected Character character;
        protected StateMachine stateMachine;
        protected Context context;

        protected State(Character character, StateMachine stateMachine, Context context)
        {
            this.character = character;
            this.stateMachine = stateMachine;
            this.context = context;
        }

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void Update() { }

        public virtual void FixedUpdate() { }
    }
}