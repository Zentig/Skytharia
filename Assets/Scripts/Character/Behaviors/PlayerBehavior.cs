using UnityEngine;

namespace Skytharia.Character.Behaviors
{
    public class PlayerBehavior : MovingBehavior
    {
        [Header("Player Setup")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        
        private PlayerContext _context;

        protected override CharacterContext Context => _context;

        private void Start()
        {
            _context = new PlayerContext();
            BuildContext();
        }

        protected override void BuildContext()
        {
            base.BuildContext();
            _context.WalkSpeed = walkSpeed;
            _context.RunSpeed = runSpeed;
        }
    }

    public class PlayerContext : MovingContext
    {
        public float WalkSpeed;
        public float RunSpeed;


        private readonly CharacterState[] _states =
        {
            new PlayerIdleState(),
            new PlayerWalkState(),
            new PlayerRunState()
        };

        protected override CharacterState[] StateList => _states;
        public enum State { Idle, Walk, Run }
    }

    public class PlayerState : MovingState
    {
        protected PlayerContext PC;

        public override void BeginState(CharacterContext context)
        {
            base.BeginState(context);
            PC = Context as PlayerContext;
        }
    }

    public class PlayerIdleState : PlayerState
    {
        public override void BeginState(CharacterContext context)
        {
            base.BeginState(context);
            PC.IsMoving = false;
        }
    }

    public class PlayerWalkState : PlayerState
    {
        public override void BeginState(CharacterContext context)
        {
            base.BeginState(context);
            PC.IsMoving = true;
            PC.Speed = PC.WalkSpeed;
        }
    }

    public class PlayerRunState : PlayerState
    {
        public override void BeginState(CharacterContext context)
        {
            base.BeginState(context);
            PC.IsMoving = true;
            PC.Speed = PC.RunSpeed;
        }
    }
}