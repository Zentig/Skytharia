using UnityEngine;

namespace Skytharia.Enemy
{
    /**
     * <summary>Class created for the Bomb Enemy.</summary>
     */
    public class BombEnemy: WalkingEnemy
    {
        [Header("Bomb Enemy Control Settings")] 
        [SerializeField] [Tooltip("Walk Speed while idling")]
        private float idleSpeed;
        [SerializeField] [Tooltip("Walk Speed while following player")]
        private float attackSpeed;
        [SerializeField] [Tooltip("Radius to detect the player")]
        private float visionRadius;
        [SerializeField] [Tooltip("Distance from player to start explosion")]
        private float explodeRadius;
        [SerializeField] [Tooltip("Radius from initial position to wander around while idling")]
        private float wanderRadius;
        [SerializeField] [Tooltip("Time to stand in place before wandering around")]
        private float idleTime;

        private BombEnemyContext _context;

        protected override EnemyContext Context => _context;

        protected override void Start()
        {
            base.Start();
            _context = new BombEnemyContext();
            SetupContext(_context);
        }

        private Transform FindPlayerTransform()
        {
            GameObject player = GameObject.FindWithTag("Player");
            return player == null ? null : player.transform;
        }

        protected override void SetupContext(EnemyContext ctx)
        {
            base.SetupContext(ctx);
            if (ctx is BombEnemyContext context)
            {
                context.IdleTime = idleTime;
                context.PlayerTransform = FindPlayerTransform();
                context.VisionRadius = visionRadius;
                context.AnchorPosition = transform.position;
                context.WanderRadius = wanderRadius;
                context.IdleSpeed = idleSpeed;
                context.ChaseSpeed = attackSpeed;
                context.ExplodeRadius = explodeRadius;
            }
            ctx.SetState((int)BombEnemyContext.EnemyStates.Idle);
        }

        protected class BombEnemyContext : WalkingEnemyContext
        {
            /** <summary>Idle Timer</summary> */
            public float IdleTime;
            /** <summary>Transform of player object</summary> */
            public Transform PlayerTransform;
            /** <summary>Distance the enemy can 'see' the player in</summary> */
            public float VisionRadius;
            /** <summary>Initial position for the enemy. Will always wander to a radius around it</summary> */
            public Vector2 AnchorPosition;
            /** <summary>Maximum distance from anchor position enemy will wander while in idle</summary> */
            public float WanderRadius;
            /** <summary>Idle Walk Speed</summary> */
            public float IdleSpeed;
            /** <summary>Player Chasing speed</summary> */
            public float ChaseSpeed;
            /** <summary>Explode Start Radius</summary> */
            public float ExplodeRadius;
            
            /** <summary>Single queue of created states to avoid reallocation</summary> */
            private readonly EnemyState[] _stateList = {
                new BombEnemyIdleState(),
                new BombEnemyAttackState(),
                new BombEnemyExplodeState(),
                new BombEnemyDeathState()
            };

            protected override EnemyState[] StateList => _stateList;
            
            /** <summary>List of items to play int the Animator's State parameter</summary> */
            public enum AnimationStates { Idle, Walk, Explode, Death }
            /** <summary>List of states that this enemy uses</summary> */
            public enum EnemyStates { Idle, Attack, Explode, Death }

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
        protected class BombEnemyState : WalkingState
        {
            public override void BeginState(EnemyContext context)
            {
                base.BeginState(context);
                WalkingContext.IsWalking = false;
            }

            protected BombEnemyContext BombContext => Context as BombEnemyContext;

            public override void Hit(float amount)
            {
                base.Hit(amount);
                Context.CurrentLife -= amount;
                if(Context.CurrentLife <= 0)
                    Context.SetState((int)BombEnemyContext.EnemyStates.Death);
            }
        }

        /**
         * <summary>Bomb's Idle State> Bomb switches back and forth between idle and walking animations,
         * wandering around in a preset radius from its original position. If it loses track of the player,
         * returns to the original wandering position.</summary>
         */
        protected class BombEnemyIdleState : BombEnemyState
        {
            private float _timeLeft;
            private bool _isWandering;
            
            public override void BeginState(EnemyContext context)
            {
                base.BeginState(context);
                BombContext.SetAnimation(BombEnemyContext.AnimationStates.Idle);
                WalkingContext.WalkSpeed = BombContext.IdleSpeed;
                _timeLeft = BombContext.IdleTime;
                _isWandering = false;
            }

            public override void Update()
            {
                base.Update();
                float dist = ((Vector2)BombContext.PlayerTransform.position - Context.Body.position).magnitude;
                if (dist < BombContext.VisionRadius)
                    Context.SetState((int)BombEnemyContext.EnemyStates.Attack);

                if (!_isWandering)
                {
                    _timeLeft -= Time.deltaTime;
                    if (_timeLeft < 0)
                    {
                        WalkingContext.TargetPosition = TargetPoint;
                        WalkingContext.IsWalking = true;
                        _isWandering = true;
                        BombContext.SetAnimation(BombEnemyContext.AnimationStates.Walk);
                    }
                }
                else
                {
                    if (WalkingContext.IsAtTarget)
                    {
                        WalkingContext.IsWalking = false;
                        _isWandering = false;
                        _timeLeft = BombContext.IdleTime;
                        BombContext.SetAnimation(BombEnemyContext.AnimationStates.Idle);
                    }
                }
            }

            private Vector2 TargetPoint
            {
                get
                {
                    float rad = Random.Range(0, BombContext.WanderRadius);
                    float angle = Random.Range(180, -180.0f) * Mathf.Deg2Rad;
                    return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * rad + BombContext.AnchorPosition;
                }
            }
        }

        /**
         * <summary>Bomb's Attacking State</summary>
         */
        protected class BombEnemyAttackState : BombEnemyState
        {
            public override void BeginState(EnemyContext context)
            {
                base.BeginState(context);
                BombContext.SetAnimation(BombEnemyContext.AnimationStates.Walk);
                WalkingContext.WalkSpeed = BombContext.ChaseSpeed;
                WalkingContext.IsWalking = true;
            }

            public override void Update()
            {
                base.Update();
                Vector2 player = BombContext.PlayerTransform.position;
                float dist = (player - Context.Body.position).magnitude;
                if (dist <= BombContext.ExplodeRadius)
                {
                    Context.SetState((int)BombEnemyContext.EnemyStates.Explode);
                }
                else if(dist > BombContext.VisionRadius)
                {
                    Context.SetState((int)BombEnemyContext.EnemyStates.Idle);
                }
                else
                {
                    WalkingContext.TargetPosition = player;
                }
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
            
            public override void AnimationEnded()
            {
                base.AnimationEnded();
                Destroy(Context.Body.gameObject);
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
            
            public override void AnimationEnded()
            {
                base.AnimationEnded();
                Destroy(Context.Body.gameObject);
            }
        }
    }
}