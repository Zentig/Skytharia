using UnityEngine;

namespace Skytharia.Enemy
{
    /**
     * <summary>Base class for all Enemy types. Inherit EnemyBase and EnemyContext and add all
     * relevant states by Inheriting EnemyState.</summary>
     */
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header("Enemy Control Settings")]
        [SerializeField] [Tooltip("Max Life for Enemy")] 
        protected float MaxLife;
        [Header("Enemy Internal Links")]
        [SerializeField] [Tooltip("Link to RigidBody controlling object")] 
        protected Rigidbody2D Body;
        [SerializeField] [Tooltip("Link to Animation Controller object with an EnemyAnimationMarker attached")] 
        protected EnemyAnimationMarker Ani;

        /**
         * <summary>Override to allow derived contexts to be used by the base functions.</summary>
         */
        protected abstract EnemyContext Context { get; }

        /**
         * <summary>Base Start method. Setup the Animation markers.</summary>
         */
        protected virtual void Start()
        {
            Ani.SetEnemyBase(this);
        }

        /**
         * <summary>Base Update method. Calls Update on the current State.</summary>
         */
        protected virtual void Update()
        {
            Context.CurrentState.Update();
        }

        /**
         * <summary>Base FixedUpdate method. Calls FixedUpdate on the current State</summary>
         */
        protected virtual void FixedUpdate()
        {
            Context.CurrentState.FixedUpdate();
        }

        /**
         * <summary>Event function to call at the end of a single play animation that needs to
         * interact with the base script. Must interact with an EnemyAnimationMarker
         * to collect the triggers.</summary>
         */
        public virtual void OnAnimationDone()
        {
            Context.CurrentState.AnimationEnded();
        }

        /**
         * <summary>Shortcut to set base variables on an inherited context.</summary>
         * <param name="ctx">Context stored in end point object to fill with base properties.</param>
         */
        protected virtual void SetupContext(EnemyContext ctx)
        {
            ctx.MaxLife = MaxLife;
            ctx.Body = Body;
            ctx.Ani = Ani.Animator;
            ctx.CurrentLife = MaxLife;
        }
        
        /**
         * <summary>Base Context class for Enemies. Inherit to make enemy specific.
         * All Child classes must supply a list of EnemyStates that correspond to the index
         * of the SetState method (enums highly recommended).</summary>
         */
        protected abstract class EnemyContext
        {
            /** <summary>Currently running EnemyState</summary> */
            public EnemyState CurrentState;
            /** <summary>Maximum life set for the enemy</summary> */
            public float MaxLife;
            /** <summary>RigidBody controller for this enemy</summary> */
            public Rigidbody2D Body;
            /** <summary>Animation controller for this enemy</summary> */
            public Animator Ani;
            /** <summary>Current Life remaining for this enemy</summary> */
            public float CurrentLife;

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
             * <summary>Override to provide the list of states used by this enemy.</summary>
             */
            protected abstract EnemyState[] StateList { get; } 
        }

        /**
         * <summary>Base class for all enemy states. Inherit and implement relevant functions.
         * Always call base.xxx() in overridden functions.</summary>
         */
        protected abstract class EnemyState
        {
            /** <summary>Context linked to this state</summary> */
            protected EnemyContext Context;

            /**
             * <summary>Called whenever this state becomes active. States should be pooled rather
             * than created to decrease memory impact of changes. Reset all state specific variables,
             * as they may have previously been in use. base.BeginState must always be called to
             * properly set the context.</summary>
             */
            public virtual void BeginState(EnemyContext context)
            {
                Context = context;
            }

            /**
             * <summary>Called when a state is ending. This should be used for context setting changes,
             * not for memory cleanup. Any large memory areas should be attached to the context.</summary>
             */
            public virtual void EndState() {}
            
            /**
             * <summary>Function called in place of Unity's Update function. Called automatically
             * from EnemyBase.</summary>
             */
            public virtual void Update() {}
            
            /**
             * <summary>Function called in place of Unity's FixedUpdate function. Called automatically
             * from EnemyBase.</summary>
             */
            public virtual void FixedUpdate() {}
            
            /**
             * <summary>Function called when a marked animation ends. Applicable animations
             * should be non repeating and call EnemyBase.OnAnimationDone() to activate.</summary>
             */
            public virtual void AnimationEnded() {}
        }
    }
}