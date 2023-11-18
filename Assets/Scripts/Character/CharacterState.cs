using UnityEngine;

namespace Skytharia.Character
{
    /**
     * <summary>Base MonoBehavior for all character types.</summary>
     */
    public abstract class CharacterBehavior : MonoBehaviour, IAnimationListener
    {
        [Header("Base Character Internal Links")] 
        [SerializeField] [Tooltip("Character RigidBody attachment")] private Rigidbody2D body;
        [SerializeField] [Tooltip("Character Animator attachment")] private AnimationListener ani;

        /** <summary>Override to provide a character specific context</summary> */
        protected abstract CharacterContext Context { get; }

        /** <summary>Sets all base items to character context. Always call base.BuildContext() when overriding.</summary> */
        protected virtual void BuildContext()
        {
            Context.Ani = ani.Animator;
            Context.Body = body;
        }

        /** <summary>Automatic call into the current state's Update.</summary> */
        protected virtual void Update()
        {
            Context.CurrentState.Update();
        }

        /** <summary>Automatic call into the current state's FixedUpdate.</summary> */
        protected virtual void FixedUpdate()
        {
            Context.CurrentState.FixedUpdate();
        }

        /** <summary>Automatic call into the current state's AnimationEnded.
         * Attaches through AnimationListener.</summary>
         */
        public virtual void OnAnimationFinished()
        {
            Context.CurrentState.AnimationEnded();
        }
    }
    
    /**
     * <summary>Base Context class for all Characters. Inherit to make character or group specific.
     * All Child classes must supply a list of CharacterStates that correspond to the index
     * of the SetState method (enums highly recommended).</summary>
     */
    public abstract class CharacterContext
    {
        /** <summary>Currently running State</summary> */
        public CharacterState CurrentState;
        /** <summary>RigidBody controller for this character</summary> */
        public Rigidbody2D Body;
        /** <summary>Animation controller for this character</summary> */
        public Animator Ani;

        /**
         * <summary>Gracefully changes from one state to another by running
         * all Start/End methods.</summary>
         * <param name="state">index of the next state from the StateList</param>
         */
        public void SetState(int state)
        {
            CurrentState?.EndState();
            CurrentState = StateList[state];
            CurrentState.BeginState(this);
        }
            
        /**
         * <summary>Override to provide the list of states used by this character.</summary>
         */
        protected abstract CharacterState[] StateList { get; } 
    }
    
    /**
     * <summary>Base class for all character states. Inherit and implement relevant functions.
     * Always call base.xxx() in overridden functions.</summary>
     */
    public abstract class CharacterState
    {
        /** <summary>Context linked to this state</summary> */
        protected CharacterContext Context;
        
        /**
         * <summary>Called whenever this state becomes active. States should be pooled rather
         * than created to decrease memory impact of changes. Reset all state specific variables,
         * as they may have previously been in use. base.BeginState must always be called to
         * properly set the context.</summary>
         */
        public virtual void BeginState(CharacterContext context)
        {
            Context = context;
        }

        /**
         * <summary>Called when a state is ending. This should be used for context setting changes,
         * not for memory cleanup. Any large memory areas should be attached to the context.</summary>
         */
        public virtual void EndState() {}
            
        /**
         * <summary>Function called in place of Unity's Update function. Should be called
         * from the Update of the MonoBehavior this is attached to.</summary>
         */
        public virtual void Update() {}
            
        /**
         * <summary>Function called in place of Unity's FixedUpdate function. Should be called
         * from the FixedUpdate of the MonoBehavior this is attached to.</summary>
         */
        public virtual void FixedUpdate() {}
            
        /**
         * <summary>Function called when a marked animation ends. Applicable animations
         * should be non repeating and use IAnimationListener/AnimationListener setup to activate.</summary>
         */
        public virtual void AnimationEnded() {}
    }
}
