using UnityEngine;

namespace Skytharia.Enemy
{
    /**
     * <summary>Class created for the Bomb Enemy.</summary>
     */
    public class BombEnemy: EnemyBase
    {
        private BombEnemyContext _context;

        protected override EnemyContext Context => _context;

        protected override void Start()
        {
            base.Start();
            _context = new BombEnemyContext();
            SetupContext(_context);
        }

        protected override void SetupContext(EnemyContext ctx)
        {
            base.SetupContext(ctx);
        }

        protected class BombEnemyContext : EnemyContext
        {
            /** <summary>Single queue of created states to avoid reallocation</summary> */
            private readonly EnemyState[] _stateList = {
                new BombEnemyIdleState(),
                new BombEnemyWalkState(),
                new BombEnemyExplodeState(),
                new BombEnemyDeathState()
            };

            protected override EnemyState[] StateList => _stateList;
            
            /** <summary>List of items to play int the Animator's State parameter</summary> */
            public enum AnimationStates { Idle, Walk, Explode, Death }
            /** <summary>List of states that this enemy uses</summary> */
            public enum EnemyStates { Idle, Walk, Explode, Death }

            private static readonly int StateHash = Animator.StringToHash("State");

            /**
             * <summary>Shortcut function to set the Animator to play an animation.</summary>
             * <param name="state">The AnimationState to use</param>
             */
            public void SetAnimation(AnimationStates state)
            {
                Ani.SetInteger(StateHash, (int)state);
            }
        }

        /**
         * <summary>Base class for Bomb Enemy, allows a quick convert of the context type.</summary>
         */
        protected class BombEnemyState : EnemyState
        {
            protected BombEnemyContext BombContext => Context as BombEnemyContext;
        }

        /**
         * <summary>Bomb's Idle State</summary>
         */
        protected class BombEnemyIdleState : BombEnemyState
        {
            public override void BeginState(EnemyContext context)
            {
                base.BeginState(context);
                BombContext.SetAnimation(BombEnemyContext.AnimationStates.Idle);
            }
        }

        /**
         * <summary>Bomb's Walking State</summary>
         */
        protected class BombEnemyWalkState : BombEnemyState
        {
            public override void BeginState(EnemyContext context)
            {
                base.BeginState(context);
                BombContext.SetAnimation(BombEnemyContext.AnimationStates.Walk);
            }
        }

        /**
         * <summary>Bomb's Death State</summary>
         */
        protected class BombEnemyDeathState : BombEnemyState
        {
            public override void BeginState(EnemyContext context)
            {
                base.BeginState(context);
                BombContext.SetAnimation(BombEnemyContext.AnimationStates.Death);
            }
        }

        /**
         * <summary>Bomb's Explode State</summary>
         */
        protected class BombEnemyExplodeState : BombEnemyState
        {
            public override void BeginState(EnemyContext context)
            {
                base.BeginState(context);
                BombContext.SetAnimation(BombEnemyContext.AnimationStates.Explode);
            }
        }
    }
}